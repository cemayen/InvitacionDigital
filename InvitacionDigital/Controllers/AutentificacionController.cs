using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using InvitacionDigital.Models;

namespace InvitacionDigital.Controllers
{
    public class AutentificacionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AutentificacionController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult InicioSesion()
        {
            // Si el usuario ya está autenticado, redirigir según su rol
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                    return RedirectToAction("Index", "Dashboard");
                else
                    return RedirectToAction("Index", "Cuenta");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InicioSesion(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Buscar el usuario por Username
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Username.ToLower() == model.Username.ToLower());

            // Validar usuario y contraseña (usando BCrypt)
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(model.Password, usuario.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
                return View(model);
            }

            // Crear los Claims (información de identidad que viaja en la cookie)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.NombreFamilia),
                new Claim("Username", usuario.Username),
                new Claim(ClaimTypes.Role, usuario.Rol)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true // Recordar sesión
            };

            // Iniciar Sesión (Escribir la cookie)
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // Redireccionar según el Rol del usuario
            if (usuario.Rol == "Admin")
            {
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                return RedirectToAction("Index", "Cuenta");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("InicioSesion");
        }
    }
}