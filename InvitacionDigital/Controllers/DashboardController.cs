using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InvitacionDigital.Models;

namespace InvitacionDigital.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Dashboard/Index
        public async Task<IActionResult> Index()
        {
            // Cargar familias (excluyendo administradores) con sus invitados
            var familias = await _context.Usuarios
                .Include(u => u.Invitados)
                .Where(u => u.Rol == "Invitado")
                .OrderByDescending(u => u.FechaCreacion)
                .ToListAsync();

            return View(familias);
        }

        // GET: /Dashboard/RegistroFamilia
        [HttpGet]
        public IActionResult RegistroFamilia()
        {
            return View();
        }

        // POST: /Dashboard/RegistroFamilia
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistroFamilia(RegistroFamiliaViewModel model)
        {
            // 1. Validar requeridos del modelo (comprueba [Required] en Username)
            if (!ModelState.IsValid)
                return View(model);

            // 2. Validar que el username sea obligatorio explícitamente por seguridad
            if (string.IsNullOrWhiteSpace(model.Username))
            {
                ModelState.AddModelError("Username", "El nombre de usuario es obligatorio.");
                return View(model);
            }

            // 3. Validar unicidad (ignora mayúsculas/minúsculas)
            bool existeUsuario = await _context.Usuarios
                .AnyAsync(u => u.Username.ToLower() == model.Username.Trim().ToLower());

            if (existeUsuario)
            {
                ModelState.AddModelError("Username", "El nombre de usuario ya está registrado por otra familia.");
                return View(model);
            }

            // Crear la entidad Familia/Usuario
            var familia = new Usuario
            {
                NombreFamilia = model.NombreFamilia,
                Username = model.Username.Trim(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                CantidadBoletos = model.CantidadBoletos,
                OpcionCabana = model.OpcionCabana,
                FechaLimite = model.FechaLimite,
                Rol = "Invitado"
            };

            for (int i = 1; i <= model.CantidadBoletos; i++)
            {
                familia.Invitados.Add(new Invitado
                {
                    NombreCompleto = null,
                    Confirmado = false
                });
            }

            _context.Usuarios.Add(familia);
            await _context.SaveChangesAsync();

            TempData["Exito"] = $"Familia '{familia.NombreFamilia}' creada con {familia.CantidadBoletos} boletos.";
            return RedirectToAction("Index");
        }

        // GET: /Dashboard/EditarFamilia/5
        [HttpGet]
        public async Task<IActionResult> EditarFamilia(int id)
        {
            var familia = await _context.Usuarios
                .Include(u => u.Invitados)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (familia == null)
                return NotFound();

            return View(familia);
        }

        // POST: /Dashboard/EditarFamilia
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarFamilia(Usuario familiaEditada, List<Invitado>? Invitados, string? NuevaPassword)
        {
            var familiaBd = await _context.Usuarios
                .Include(u => u.Invitados)
                .FirstOrDefaultAsync(u => u.Id == familiaEditada.Id);

            if (familiaBd == null)
                return NotFound();

            // 1. Validar Username obligatorio
            if (string.IsNullOrWhiteSpace(familiaEditada.Username))
            {
                TempData["Error"] = "El nombre de usuario es obligatorio.";
                return RedirectToAction("ListaInvitados");
            }

            string usernameLimpio = familiaEditada.Username.Trim();

            // 2. Validar Unicidad de Username
            bool existeEnOtroUsuario = await _context.Usuarios
                .AnyAsync(u => u.Username.ToLower() == usernameLimpio.ToLower() && u.Id != familiaEditada.Id);

            if (existeEnOtroUsuario)
            {
                TempData["Error"] = $"El usuario '{usernameLimpio}' ya pertenece a otra familia.";
                return RedirectToAction("ListaInvitados");
            }

            // Actualizar datos principales
            familiaBd.NombreFamilia = familiaEditada.NombreFamilia;
            familiaBd.Username = usernameLimpio;
            familiaBd.OpcionCabana = familiaEditada.OpcionCabana;
            familiaBd.FechaLimite = familiaEditada.FechaLimite;

            // Actualizar contraseña si se ingresó
            if (!string.IsNullOrWhiteSpace(NuevaPassword))
            {
                familiaBd.PasswordHash = BCrypt.Net.BCrypt.HashPassword(NuevaPassword);
            }

            // 3. ACTUALIZAR LOS NOMBRES DE LOS INVITADOS EXISTENTES
            if (Invitados != null && Invitados.Any())
            {
                foreach (var invForm in Invitados)
                {
                    var invitadoBd = familiaBd.Invitados.FirstOrDefault(i => i.Id == invForm.Id);
                    if (invitadoBd != null)
                    {
                        string? nuevoNombre = string.IsNullOrWhiteSpace(invForm.NombreCompleto) 
                            ? null 
                            : invForm.NombreCompleto.Trim();

                        invitadoBd.NombreCompleto = nuevoNombre;
                        // Si tiene nombre, queda confirmado
                        invitadoBd.Confirmado = !string.IsNullOrWhiteSpace(nuevoNombre);
                    }
                }
            }

            // 4. Ajustar la cantidad de boletos si cambió la cifra total
            if (familiaBd.CantidadBoletos != familiaEditada.CantidadBoletos)
            {
                int diferencia = familiaEditada.CantidadBoletos - familiaBd.CantidadBoletos;

                if (diferencia > 0)
                {
                    for (int i = 0; i < diferencia; i++)
                    {
                        familiaBd.Invitados.Add(new Invitado { NombreCompleto = null, Confirmado = false });
                    }
                }
                else if (diferencia < 0)
                {
                    var aEliminar = familiaBd.Invitados.Skip(familiaEditada.CantidadBoletos).ToList();
                    foreach (var inv in aEliminar)
                    {
                        _context.Invitados.Remove(inv);
                    }
                }

                familiaBd.CantidadBoletos = familiaEditada.CantidadBoletos;
            }

            await _context.SaveChangesAsync();

            TempData["Exito"] = "Familia e invitados actualizados correctamente.";
            return RedirectToAction("ListaInvitados");
        }
        // GET: /Dashboard/ListaInvitados
        [HttpGet]
        public async Task<IActionResult> ListaInvitados()
        {
            // Consultamos todas las familias (rol Invitado) con sus respectivos invitados/boletos
            var familias = await _context.Usuarios
                .Include(u => u.Invitados)
                .Where(u => u.Rol == "Invitado")
                .OrderBy(u => u.NombreFamilia)
                .ToListAsync();

            return View(familias);
        }


        // POST: /Dashboard/EliminarFamilia
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarFamilia(int id)
        {
            var familia = await _context.Usuarios
                .Include(u => u.Invitados)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (familia == null)
            {
                TempData["Error"] = "La familia no existe o ya fue eliminada.";
                return RedirectToAction("Index");
            }

            // Removemos invitados explícitamente y luego el registro de la familia
            _context.Invitados.RemoveRange(familia.Invitados);
            _context.Usuarios.Remove(familia);

            await _context.SaveChangesAsync();

            TempData["Exito"] = $"La familia '{familia.NombreFamilia}' fue eliminada correctamente.";
            return RedirectToAction("Index");
        }
    }
}