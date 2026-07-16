using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIGEBOM.Datos.Models;
using SIGEBOM.Negocio.Interfaces;
using SIGEBOM.Negocio.ViewModels;
namespace SIGEBOM.Presentacion.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IRolService _rolService;
        private readonly IBomberoService _bomberoService;

        public UsuariosController(
            IUsuarioService usuarioService,
            IRolService rolService,
            IBomberoService bomberoService)
        {
            _usuarioService = usuarioService;
            _rolService = rolService;
            _bomberoService = bomberoService;
        }

        //=========================================
        // INDEX
        //=========================================

        public async Task<IActionResult> Index(string? buscar)
        {
            ViewData["Buscar"] = buscar;

            var usuarios = await _usuarioService.ObtenerTodos(buscar);

            return View(usuarios);
        }

        //=========================================
        // DETAILS
        //=========================================

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var usuario = await _usuarioService.ObtenerPorId(id.Value);

            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        //=========================================
        // CREATE GET
        //=========================================

        public async Task<IActionResult> Create()
        {
            await CargarRoles();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                foreach (var item in ModelState)
                {
                    if (item.Value.Errors.Count > 0)
                    {
                        Console.WriteLine($"CAMPO: {item.Key}");

                        foreach (var error in item.Value.Errors)
                        {
                            Console.WriteLine($"ERROR: {error.ErrorMessage}");
                        }
                    }
                }

                await CargarRoles();

                return View(usuario);
            }

            var resultado = await _usuarioService.Crear(usuario);

            if (!resultado.Exitoso)
            {
                ModelState.AddModelError("", resultado.Mensaje);

                await CargarRoles();

                return View(usuario);
            }

            TempData["Success"] = resultado.Mensaje;

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var usuario = await _usuarioService.ObtenerPorId(id.Value);

            if (usuario == null)
                return NotFound();

            await CargarRoles();

            return View(usuario);
        }

        //=========================================
        // EDIT POST
        //=========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Usuario usuario)
        {
            if (id != usuario.IdUsuario)
                return NotFound();

            ModelState.Remove(nameof(Usuario.Bombero));
            ModelState.Remove(nameof(Usuario.Rol));
            ModelState.Remove(nameof(Usuario.Contraseña));
            ModelState.Remove(nameof(Usuario.ConfirmarContraseña));

            if (!ModelState.IsValid)
            {
                await CargarRoles();
                return View(usuario);
            }

            var resultado = await _usuarioService.Actualizar(usuario);

            if (!resultado.Exitoso)
            {
                ModelState.AddModelError("", resultado.Mensaje);

                await CargarRoles();

                return View(usuario);
            }

            TempData["Success"] = resultado.Mensaje;

            return RedirectToAction(nameof(Index));
        }

        //=========================================
        // DESACTIVAR GET
        //=========================================

        public async Task<IActionResult> Desactivar(int? id)
        {
            if (id == null)
                return NotFound();

            var usuario = await _usuarioService.ObtenerPorId(id.Value);

            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        //=========================================
        // DESACTIVAR POST
        //=========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Desactivar(int id)
        {
            var resultado = await _usuarioService.Desactivar(id);

            TempData[resultado.Exitoso ? "Success" : "Error"] =
                resultado.Mensaje;

            return RedirectToAction(nameof(Index));
        }

        //=========================================
        // BUSCAR BOMBEROS (MODAL)
        //=========================================

        public async Task<IActionResult> BuscarBomberos(string? buscar)
        {
            var bomberos = await _bomberoService.ObtenerBomberosDisponibles(buscar);

            return PartialView("~/Views/Shared/_TablaBomberos.cshtml", bomberos);
        }

        //=========================================
        // CAMBIAR CONTRASEÑA GET
        //=========================================

        //=========================================
        // CAMBIAR CONTRASEÑA GET
        //=========================================

        public async Task<IActionResult> CambiarPassword(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _usuarioService.ObtenerPorId(id.Value);

            if (usuario == null)
            {
                return NotFound();
            }

            var modelo = new CambiarPasswordViewModel
            {
                IdUsuario = usuario.IdUsuario,
                NombreUsuario = usuario.NombreUsuario
            };

            return View(modelo);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarPassword(CambiarPasswordViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var resultado = await _usuarioService.CambiarPassword(
                modelo.IdUsuario,
                modelo.NuevaContraseña);

            if (!resultado.Exitoso)
            {
                ModelState.AddModelError(string.Empty, resultado.Mensaje);

                return View(modelo);
            }

            TempData["Success"] = resultado.Mensaje;

            return RedirectToAction(nameof(Index));
        }


        // MÉTODOS PRIVADOS
      

        private async Task CargarRoles()
        {
            var roles = await _rolService.ObtenerTodos(null);

            ViewBag.IdRol = new SelectList(
                roles,
                "IdRol",
                "NombreRol");
        }
    }
}