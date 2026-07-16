using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBOM.Negocio.Interfaces;
using SIGEBOM.Negocio.ViewModels;
using System.Security.Claims;

namespace SIGEBOM.Presentacion.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public AccountController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        //=========================================
        // LOGIN (GET)
        //=========================================

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        //=========================================
        // LOGIN (POST)
        //=========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var usuario = await _usuarioService.Login(
                modelo.NombreUsuario,
                modelo.Contraseña);

            if (usuario == null)
            {
                ModelState.AddModelError("", "Usuario o contraseña incorrectos.");

                return View(modelo);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),

                new Claim(ClaimTypes.Name, usuario.NombreUsuario),

                new Claim(ClaimTypes.Role, usuario.Rol?.NombreRol ?? ""),

                new Claim("IdUsuario", usuario.IdUsuario.ToString()),

                new Claim("IdBombero", usuario.IdBombero.ToString()),

                new Claim("NombreCompleto",
                    usuario.Bombero?.NombreCompleto ?? usuario.NombreUsuario),

                new Claim("Rol",
                    usuario.Rol?.NombreRol ?? "")
            };

            var identidad = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identidad);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = modelo.Recordarme,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
                    AllowRefresh = true
                });

            TempData["Success"] =
                $"Bienvenido {usuario.Bombero?.NombreCompleto ?? usuario.NombreUsuario}";

            return RedirectToAction("Index", "Home");
        }

        //=========================================
        // LOGOUT
        //=========================================

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(Login));
        }

        //=========================================
        // ACCESO DENEGADO
        //=========================================

        [HttpGet]
        public IActionResult AccesoDenegado()
        {
            return View();
        }
    }
}