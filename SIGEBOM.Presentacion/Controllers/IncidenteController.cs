using Microsoft.AspNetCore.Mvc;
using SIGEBOM.Negocio.Interfaces;
using SIGEBOM.Negocio.ViewModels;

namespace SIGEBOM.Presentacion.Controllers
{
    public class IncidenteController : Controller
    {
        private readonly IIncidenteService _incidenteService;

        public IncidenteController(IIncidenteService incidenteService)
        {
            _incidenteService = incidenteService;
        }

        //=========================================
        // INDEX
        //=========================================

        public async Task<IActionResult> Index()
        {
            var lista = await _incidenteService.ObtenerTodos();

            return View(lista);
        }

        //=========================================
        // DETAILS
        //=========================================

        public async Task<IActionResult> Details(int id)
        {
            var incidente = await _incidenteService.ObtenerPorId(id);

            if (incidente == null)
                return NotFound();

            return View(incidente);
        }

        //=========================================
        // CREATE
        //=========================================

        public async Task<IActionResult> Create(int idLlamada)
        {
            var model = await _incidenteService.ObtenerDesdeLlamada(idLlamada);

            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IncidenteViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var resultado = await _incidenteService.Crear(model);

            if (!resultado.Exitoso)
            {
                ModelState.AddModelError("", resultado.Mensaje);

                return View(model);
            }

            TempData["Success"] = resultado.Mensaje;

            return RedirectToAction(nameof(Index));
        }

        //=========================================
        // EDIT
        //=========================================

        public async Task<IActionResult> Edit(int id)
        {
            var model = await _incidenteService.ObtenerViewModelEditar(id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            IncidenteViewModel model)
        {
            if (id != model.IdIncidente)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            var resultado = await _incidenteService.Actualizar(model);

            if (!resultado.Exitoso)
            {
                ModelState.AddModelError("", resultado.Mensaje);

                return View(model);
            }

            TempData["Success"] = resultado.Mensaje;

            return RedirectToAction(nameof(Index));
        }
    }
}