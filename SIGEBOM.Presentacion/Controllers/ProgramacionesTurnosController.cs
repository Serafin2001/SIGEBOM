using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIGEBOM.Datos.Models;
using SIGEBOM.Negocio.Interfaces;
using SIGEBOM.Negocio.Services;
using SIGEBOM.Negocio.ViewModels;
using System.Security.Claims;

namespace SIGEBOM.Presentacion.Controllers
{
    [Authorize(Roles = "Administrador,Oficial")]
    public class ProgramacionesTurnosController : Controller
    {

        private readonly IProgramacionTurnoService _programacionTurnoService;
        private readonly ITurnoService _turnoService;
        private readonly IBomberoService _bomberoService;

        public ProgramacionesTurnosController(
            IProgramacionTurnoService programacionTurnoService,
            ITurnoService turnoService,
            IBomberoService bomberoService)
        {
            _programacionTurnoService = programacionTurnoService;
            _turnoService = turnoService;
            _bomberoService = bomberoService;
        }
        public async Task<IActionResult> Index(DateOnly? fecha)
        {
            var lista = await _programacionTurnoService.ObtenerTodos(fecha);

            ViewBag.Fecha = fecha;

            return View(lista);
        }



        public async Task<IActionResult> Details(int id)
        {
            var programacion = await _programacionTurnoService.ObtenerPorId(id);

            if (programacion == null)
                return NotFound();

            return View(programacion);
        }


        public async Task<IActionResult> Create()
        {
            var modelo = new ProgramacionTurnoViewModel
            {
                Fecha = DateOnly.FromDateTime(DateTime.Today)
            };

            await CargarCombos(modelo);

            return View(modelo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProgramacionTurnoViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                await CargarCombos(modelo);
                return View(modelo);
            }

            var idUsuarioClaim = User.FindFirst("IdUsuario");

            if (idUsuarioClaim == null)
            {
                ModelState.AddModelError("", "No fue posible identificar el usuario autenticado.");
                await CargarCombos(modelo);
                return View(modelo);
            }

            var programacion = new ProgramacionTurno
            {
                Fecha = modelo.Fecha,
                IdTurno = modelo.IdTurno,
                IdEncargado = modelo.IdEncargado,
                Observacion = modelo.Observacion,
                Estado = "Programado",
                FechaCreacion = DateTime.Now,
                IdUsuarioCreador = int.Parse(idUsuarioClaim.Value)
            };

            var resultado = await _programacionTurnoService.Crear(
                programacion,
                modelo.BomberosSeleccionados);

            if (!resultado.Exitoso)
            {
                ModelState.AddModelError("", resultado.Mensaje);

                await CargarCombos(modelo);

                return View(modelo);
            }

            TempData["Success"] = resultado.Mensaje;

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var vm = await _programacionTurnoService.ObtenerViewModelEditar(id);

            if (vm == null)
                return NotFound();

            return View(vm);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProgramacionTurnoViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await CargarCombos(vm);
                return View(vm);
            }

            var programacion = new ProgramacionTurno
            {
                IdProgramacionTurno = vm.IdProgramacionTurno,
                Fecha = vm.Fecha,
                IdTurno = vm.IdTurno,
                IdEncargado = vm.IdEncargado,
                Observacion = vm.Observacion
            };

            var resultado = await _programacionTurnoService.Actualizar(
                programacion,
                vm.BomberosSeleccionados);

            if (!resultado.Exitoso)
            {
                ModelState.AddModelError("", resultado.Mensaje);

                await CargarCombos(vm);

                return View(vm);
            }

            TempData["Success"] = resultado.Mensaje;

            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var resultado = await _programacionTurnoService.Desactivar(id);

            TempData["Success"] = resultado.Mensaje;

            return RedirectToAction(nameof(Index));
        }


        private async Task CargarCombos(ProgramacionTurnoViewModel modelo)
        {
            modelo.Turnos = await _turnoService.ObtenerTodos(null);
        }
    }
}