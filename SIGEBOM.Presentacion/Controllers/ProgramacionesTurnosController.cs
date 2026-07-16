using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIGEBOM.Datos.Models;
using SIGEBOM.Negocio.Interfaces;
using SIGEBOM.Negocio.ViewModels;

namespace SIGEBOM.Presentacion.Controllers
{
    [Authorize]
    public class ProgramacionesTurnosController : Controller
    {
        private readonly IProgramacionTurnoService _programacionService;
        private readonly ITurnoService _turnoService;
        private readonly IBomberoService _bomberoService;

        public ProgramacionesTurnosController(
            IProgramacionTurnoService programacionService,
            ITurnoService turnoService,
            IBomberoService bomberoService)
        {
            _programacionService = programacionService;
            _turnoService = turnoService;
            _bomberoService = bomberoService;
        }

        //=========================================
        // INDEX
        //=========================================

        public async Task<IActionResult> Index(DateTime? fecha)
        {
            var lista = await _programacionService.ObtenerTodos(fecha);

            ViewBag.Fecha = fecha;

            return View(lista);
        }

        //=========================================
        // CREATE GET
        //=========================================

        public async Task<IActionResult> Create()
        {
            await CargarTurnos();

            return View();
        }

        //=========================================
        // CREATE POST
        //=========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProgramacionTurnoViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                await CargarTurnos();
                return View(modelo);
            }

            var programacion = new ProgramacionTurno
            {
               
                IdTurno = modelo.IdTurno,
                IdEncargado = modelo.IdEncargado
            };

            var resultado = await _programacionService.Crear(
                programacion,
                modelo.BomberosSeleccionados);

            if (!resultado.Exitoso)
            {
                ModelState.AddModelError("", resultado.Mensaje);

                await CargarTurnos();

                return View(modelo);
            }

            TempData["Success"] = resultado.Mensaje;

            return RedirectToAction(nameof(Index));
        }

        //=========================================
        // MÉTODO PRIVADO
        //=========================================

        private async Task CargarTurnos()
        {
            var turnos = await _turnoService.ObtenerTodos(null);

            ViewBag.IdTurno = new SelectList(
                turnos,
                "IdTurno",
                "NombreTurno");
        }
    }
}