using Microsoft.AspNetCore.Mvc;
using SIGEBOM.Negocio.Interfaces;
using SIGEBOM.Negocio.ViewModels;

namespace SIGEBOM.Presentacion.Controllers
{
    public class TipoIncidenteController : Controller
    {
        private readonly ITipoIncidenteService _tipoIncidenteService;

        public TipoIncidenteController(
            ITipoIncidenteService tipoIncidenteService)
        {
            _tipoIncidenteService = tipoIncidenteService;
        }

        //=========================================
        // INDEX
        //=========================================

        public async Task<IActionResult> Index()
        {
            var lista = await _tipoIncidenteService.ObtenerTodos();

            return View(lista);
        }

        //=========================================
        // DETAILS
        //=========================================

        public async Task<IActionResult> Details(int id)
        {
            var tipo = await _tipoIncidenteService.ObtenerPorId(id);

            if (tipo == null)
                return NotFound();

            return View(tipo);
        }

        //=========================================
        // CREATE
        //=========================================

        public IActionResult Create()
        {
            return View(new TipoIncidenteViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TipoIncidenteViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var resultado = await _tipoIncidenteService.Crear(model);

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
            var model = await _tipoIncidenteService.ObtenerViewModelEditar(id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            TipoIncidenteViewModel model)
        {
            if (id != model.IdTipoIncidente)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            var resultado = await _tipoIncidenteService.Actualizar(model);

            if (!resultado.Exitoso)
            {
                ModelState.AddModelError("", resultado.Mensaje);
                return View(model);
            }

            TempData["Success"] = resultado.Mensaje;

            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int id, string estado)
        {
            var resultado = await _tipoIncidenteService.CambiarEstado(id, estado);

            return Json(resultado);
        }
    }
}