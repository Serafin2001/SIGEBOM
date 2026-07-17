using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBOM.Datos.Models;
using SIGEBOM.Negocio.Interfaces;

namespace SIGEBOM.Presentacion.Controllers
{
    [Authorize(Roles = "Administrador,Oficial")]
    public class TurnosController : Controller
    {
        private readonly ITurnoService _turnoService;

        public TurnosController(ITurnoService turnoService)
        {
            _turnoService = turnoService;
        }

        //=========================================
        // INDEX
        //=========================================

        public async Task<IActionResult> Index(string? buscar)
        {
            var turnos = await _turnoService.ObtenerTodos(buscar);

            ViewBag.Buscar = buscar;

            return View(turnos);
        }

        //=========================================
        // DETAILS
        //=========================================

        public async Task<IActionResult> Details(int id)
        {
            var turno = await _turnoService.ObtenerPorId(id);

            if (turno == null)
            {
                return NotFound();
            }

            return View(turno);
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
        public async Task<IActionResult> Create(Turno turno)
        {
            if (!ModelState.IsValid)
            {
                return View(turno);
            }

            var resultado = await _turnoService.Crear(turno);

            if (!resultado.Exitoso)
            {
                ModelState.AddModelError("", resultado.Mensaje);

                return View(turno);
            }

            TempData["Success"] = resultado.Mensaje;

            return RedirectToAction(nameof(Index));
        }

        //=========================================
        // EDIT GET
        //=========================================

        public async Task<IActionResult> Edit(int id)
        {
            var turno = await _turnoService.ObtenerPorId(id);

            if (turno == null)
            {
                return NotFound();
            }

            return View(turno);
        }

        //=========================================
        // EDIT POST
        //=========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Turno turno)
        {
            if (!ModelState.IsValid)
            {
                return View(turno);
            }

            var resultado = await _turnoService.Actualizar(turno);

            if (!resultado.Exitoso)
            {
                ModelState.AddModelError("", resultado.Mensaje);

                return View(turno);
            }

            TempData["Success"] = resultado.Mensaje;

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var resultado = await _turnoService.Desactivar(id);

            TempData["Success"] = resultado.Mensaje;

            return RedirectToAction(nameof(Index));
        }
    }
}