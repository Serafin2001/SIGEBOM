using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIGEBOM.Datos.Models;
using SIGEBOM.Negocio.Interfaces;

namespace SIGEBOM.Presentacion.Controllers
{
    [Authorize(Roles = "Administrador,Oficial")]
    public class BomberosController : Controller
    {
        private readonly IBomberoService _bomberoService;
        private readonly ICargoService _cargoService;
        private readonly IRangoService _rangoService;

        public BomberosController(
            IBomberoService bomberoService,
            ICargoService cargoService,
            IRangoService rangoService)
        {
            _bomberoService = bomberoService;
            _cargoService = cargoService;
            _rangoService = rangoService;
        }



        public async Task<IActionResult> Index(string? buscar)
        {
            ViewData["Buscar"] = buscar;

            var bomberos = await _bomberoService.ObtenerTodos(buscar);

            return View(bomberos);
        }



        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var bombero = await _bomberoService.ObtenerPorId(id.Value);

            if (bombero == null)
                return NotFound();

            return View(bombero);
        }



        public async Task<IActionResult> Create()
        {
            await CargarCombos();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Bombero bombero)
        {
            ModelState.Remove(nameof(Bombero.Rango));
            ModelState.Remove(nameof(Bombero.Cargo));
            ModelState.Remove(nameof(Bombero.Usuario));

            if (!ModelState.IsValid)
            {
                await CargarCombos();
                return View(bombero);
            }

            var resultado = await _bomberoService.Crear(bombero);

            if (!resultado.Exitoso)
            {
                ModelState.AddModelError("", resultado.Mensaje);

                await CargarCombos();

                return View(bombero);
            }

            TempData["Success"] = resultado.Mensaje;

            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var bombero = await _bomberoService.ObtenerPorId(id.Value);

            if (bombero == null)
                return NotFound();

            await CargarCombos();

            return View(bombero);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Bombero bombero)
        {
            if (id != bombero.IdBombero)
                return NotFound();

            ModelState.Remove(nameof(Bombero.Rango));
            ModelState.Remove(nameof(Bombero.Cargo));
            ModelState.Remove(nameof(Bombero.Usuario));

            if (!ModelState.IsValid)
            {
                await CargarCombos();
                return View(bombero);
            }

            var resultado = await _bomberoService.Actualizar(bombero);

            if (!resultado.Exitoso)
            {
                ModelState.AddModelError("", resultado.Mensaje);

                await CargarCombos();

                return View(bombero);
            }

            TempData["Success"] = resultado.Mensaje;

            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Desactivar(int? id)
        {
            if (id == null)
                return NotFound();

            var bombero = await _bomberoService.ObtenerPorId(id.Value);

            if (bombero == null)
                return NotFound();

            return View(bombero);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Desactivar(int id)
        {
            var resultado = await _bomberoService.Desactivar(id);

            TempData[resultado.Exitoso ? "Success" : "Error"] = resultado.Mensaje;

            return RedirectToAction(nameof(Index));
        }

        private async Task CargarCombos()
        {
            var cargos = await _cargoService.ObtenerTodos(null);
            var rangos = await _rangoService.ObtenerTodos(null);

            ViewBag.IdCargo = new SelectList(cargos, "IdCargo", "NombreCargo");

            ViewBag.IdRango = new SelectList(rangos, "IdRango", "NombreRango");
        }




        [HttpGet]
        public async Task<IActionResult> Buscar(string? buscar)
        {
            var bomberos = await _bomberoService.ObtenerTodos(buscar);

            var resultado = bomberos
                .Where(b => b.Estado == "Activo")
                .Select(b => new
                {
                    id = b.IdBombero,
                    cedula = b.Cedula,
                    nombre = b.NombreCompleto,
                    cargo = b.Cargo != null ? b.Cargo.NombreCargo : "Sin cargo"
                })
                .ToList();

            return Json(resultado);
        }
    }
}
