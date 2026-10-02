using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting; // <-- Necesario para IWebHostEnvironment
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InvitacionDigital.Models;
using InvitacionDigital.Services;

namespace InvitacionDigital.Controllers
{
    [Authorize]
    public class CuentaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CuentaController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: /Cuenta/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out int usuarioId))
            {
                return RedirectToAction("InicioSesion", "Autentificacion");
            }

            var familia = await _context.Usuarios
                .Include(u => u.Invitados)
                .FirstOrDefaultAsync(u => u.Id == usuarioId);

            if (familia == null)
            {
                return NotFound();
            }
            
            // 1. Validar si la fecha límite ya expiró
            bool fechaExpirada = familia.FechaLimite.HasValue && DateTime.Now.Date > familia.FechaLimite.Value.Date;

            // 2. Validar si la familia YA registró a sus invitados
            // Se considera registrado si al menos un invitado tiene nombre asignado (o si todos están completos)
            bool yaRegistrados = familia.Invitados.Any(i => !string.IsNullOrWhiteSpace(i.NombreCompleto));

            // La edición se bloquea si la fecha expiró O si ya enviaron sus nombres
            ViewBag.EdicionBloqueada = fechaExpirada || yaRegistrados;
            ViewBag.YaRegistrados = yaRegistrados;
            ViewBag.FechaExpirada = fechaExpirada;

            return View(familia);
        }

        // POST: /Cuenta/GuardarInvitados
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuardarInvitados(List<Invitado> invitados)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out int usuarioId))
            {
                return RedirectToAction("InicioSesion", "Autentificacion");
            }

            var familia = await _context.Usuarios
                .Include(u => u.Invitados)
                .FirstOrDefaultAsync(u => u.Id == usuarioId);

            if (familia == null)
            {
                return NotFound();
            }

            // Validar fecha límite
            if (familia.FechaLimite.HasValue && DateTime.Now.Date > familia.FechaLimite.Value.Date)
            {
                TempData["Error"] = "La fecha límite para confirmar tus pases ha expirado.";
                return RedirectToAction("Index");
            }

            // Validar si ya habían registrado invitados anteriormente para evitar modificaciones directas por POST
            bool yaRegistrados = familia.Invitados.Any(i => !string.IsNullOrWhiteSpace(i.NombreCompleto));
            if (yaRegistrados)
            {
                TempData["Error"] = "Los invitados ya han sido registrados previamente y no pueden modificarse.";
                return RedirectToAction("Index");
            }

            // Guardar nombres de los invitados
            foreach (var invForm in invitados)
            {
                var invitadoBd = familia.Invitados.FirstOrDefault(i => i.Id == invForm.Id);
                if (invitadoBd != null && !string.IsNullOrWhiteSpace(invForm.NombreCompleto))
                {
                    invitadoBd.NombreCompleto = invForm.NombreCompleto.Trim();
                    invitadoBd.Confirmado = true;
                }
            }

            await _context.SaveChangesAsync();
            TempData["Exito"] = "Los invitados han sido registrados con éxito. Si deseas cambiar un invitado, comunícate con los novios.";

            return RedirectToAction("Index");
        }

        // GET: /Cuenta/DescargarBoleto/5
        [HttpGet]
        public async Task<IActionResult> DescargarBoleto(int id)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out int usuarioId))
                return RedirectToAction("InicioSesion", "Autentificacion");

            var invitado = await _context.Invitados
                .Include(i => i.Usuario)
                .FirstOrDefaultAsync(i => i.Id == id && i.UsuarioId == usuarioId);

            if (invitado == null || string.IsNullOrWhiteSpace(invitado.NombreCompleto))
            {
                TempData["Error"] = "El boleto aún no tiene nombre asignado.";
                return RedirectToAction("Index");
            }

            // Construir la ruta física a la imagen Boleto.png
            string rutaImagen = Path.Combine(_env.WebRootPath, "assets", "imgs", "Boleto2.png");

            byte[] pdfBytes = BoletoPdfService.GenerarBoletoPdf(
                invitado.Usuario?.NombreFamilia ?? "Invitado",
                invitado.NombreCompleto,
                rutaImagen
            );

            string nombreArchivo = $"Boleto_{invitado.NombreCompleto.Replace(" ", "_")}.pdf";
            return File(pdfBytes, "application/pdf", nombreArchivo);
        }
    }
}