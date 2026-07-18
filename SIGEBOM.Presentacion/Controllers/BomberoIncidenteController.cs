using Microsoft.AspNetCore.Mvc;
using SIGEBOM.Negocio.Interfaces;
using SIGEBOM.Negocio.ViewModels;

namespace SIGEBOM.Presentacion.Controllers
{
    public class BomberoIncidenteController : Controller
    {
        private readonly IBomberoIncidenteService _bomberoIncidenteService;

        public BomberoIncidenteController(
            IBomberoIncidenteService bomberoIncidenteService)
        {
            _bomberoIncidenteService = bomberoIncidenteService;
        }

        public async Task<IActionResult> Index(int idIncidente)
        {
            var model = await _bomberoIncidenteService
                .ObtenerPantalla(idIncidente);

            if (model == null)
                return NotFound();

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var registro = await _bomberoIncidenteService
                .ObtenerPorId(id);

            if (registro == null)
                return NotFound();

            return View(registro);
        }

        public IActionResult Create(int idIncidente)
        {
            var model = new BomberoIncidenteViewModel
            {
                IdIncidente = idIncidente
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BomberoIncidenteViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var resultado = await _bomberoIncidenteService.Crear(model);

            if (!resultado.Exitoso)
            {
                ModelState.AddModelError(string.Empty, resultado.Mensaje);
                return View(model);
            }

            TempData["Success"] = resultado.Mensaje;

            return RedirectToAction(nameof(Index),
                new { idIncidente = model.IdIncidente });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var model = await _bomberoIncidenteService
                .ObtenerViewModelEditar(id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BomberoIncidenteViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var resultado = await _bomberoIncidenteService.Actualizar(model);

            if (!resultado.Exitoso)
            {
                ModelState.AddModelError(string.Empty, resultado.Mensaje);
                return View(model);
            }

            TempData["Success"] = resultado.Mensaje;

            return RedirectToAction(nameof(Index),
                new { idIncidente = model.IdIncidente });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(int id, int idIncidente)
        {
            var resultado = await _bomberoIncidenteService
                .CambiarEstado(id);

            TempData["Success"] = resultado.Mensaje;

            return RedirectToAction(nameof(Index),
                new { idIncidente });
        }

        [HttpGet]
        public async Task<IActionResult> BuscarBomberos(string filtro = "")
        {
            var bomberos = await _bomberoIncidenteService.BuscarBomberos(filtro);

            return Json(bomberos);
        }
    }


}