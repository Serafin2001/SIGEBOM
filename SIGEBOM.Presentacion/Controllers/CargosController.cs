using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBOM.Datos.Models;
using SIGEBOM.Negocio.Interfaces;

namespace SIGEBOM.Presentacion.Controllers
{
    [Authorize]
    public class CargosController : Controller
    {
        private readonly ICargoService _cargoService;

        public CargosController(ICargoService cargoService)
        {
            _cargoService = cargoService;
        }

        //=========================================
        // INDEX
        //=========================================

        public async Task<IActionResult> Index(string? buscar)
        {
            ViewData["Buscar"] = buscar;

            var cargos = await _cargoService.ObtenerTodos(buscar);

            return View(cargos);
        }

        //=========================================
        // DETAILS
        //=========================================

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var cargo = await _cargoService.ObtenerPorId(id.Value);

            if (cargo == null)
                return NotFound();

            return View(cargo);
        }

        //=========================================
        // CREATE GET
        //=========================================

        public IActionResult Create()
        {
            return View();
        }

        //=========================================
        // CREATE POST
        //=========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cargo cargo)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"{error.Key}");

                    foreach (var e in error.Value.Errors)
                    {
                        Console.WriteLine(e.ErrorMessage);
                    }
                }

                return View(cargo);
            }

            var resultado = await _cargoService.Crear(cargo);

            if (!resultado.Exitoso)
            {
                ModelState.AddModelError("", resultado.Mensaje);
                return View(cargo);
            }

            TempData["Success"] = resultado.Mensaje;

            return RedirectToAction(nameof(Index));
        }

        //=========================================
        // EDIT GET
        //=========================================

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var cargo = await _cargoService.ObtenerPorId(id.Value);

            if (cargo == null)
                return NotFound();

            return View(cargo);
        }

        //=========================================
        // EDIT POST
        //=========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cargo cargo)
        {
            if (id != cargo.IdCargo)
                return NotFound();

            if (!ModelState.IsValid)
                return View(cargo);

            var resultado = await _cargoService.Actualizar(cargo);

            if (!resultado.Exitoso)
            {
                ModelState.AddModelError("", resultado.Mensaje);
                return View(cargo);
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

            var cargo = await _cargoService.ObtenerPorId(id.Value);

            if (cargo == null)
                return NotFound();

            return View(cargo);
        }

        //=========================================
        // DESACTIVAR POST
        //=========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Desactivar(int id)
        {
            var resultado = await _cargoService.Desactivar(id);

            TempData[resultado.Exitoso ? "Success" : "Error"] = resultado.Mensaje;

            return RedirectToAction(nameof(Index));
        }
    }
}