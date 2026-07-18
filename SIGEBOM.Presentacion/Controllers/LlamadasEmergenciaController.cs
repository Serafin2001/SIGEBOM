using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using SIGEBOM.Negocio.Interfaces;
using SIGEBOM.Negocio.ViewModels;

namespace SIGEBOM.Presentacion.Controllers
{
    public class LlamadasEmergenciaController : Controller
    {
        private readonly ILlamadaEmergenciaService _llamadaService;
        private readonly ITipoIncidenteService _tipoIncidenteService;

        public LlamadasEmergenciaController(
            ILlamadaEmergenciaService llamadaService,
            ITipoIncidenteService tipoIncidenteService)
        {
            _llamadaService = llamadaService;
            _tipoIncidenteService = tipoIncidenteService;
        }

        //=========================================
        // INDEX
        //=========================================

        public async Task<IActionResult> Index()
        {
            var lista = await _llamadaService.ObtenerTodas();

            return View(lista);
        }

        //=========================================
        // DETAILS
        //=========================================

        public async Task<IActionResult> Details(int id)
        {
            var llamada = await _llamadaService.ObtenerPorId(id);

            if (llamada == null)
                return NotFound();

            return View(llamada);
        }

        //=========================================
        // CREATE
        //=========================================

        public async Task<IActionResult> Create()
        {
            await CargarCombos();

            return View(new LlamadaEmergenciaViewModel
            {
                FechaHora = DateTime.Now,
                Estado = "Pendiente"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LlamadaEmergenciaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await CargarCombos();

                return View(model);
            }

            int idUsuario = int.Parse(
                User.FindFirst("IdUsuario")!.Value);

            var resultado =
                await _llamadaService.Crear(model, idUsuario);

            if (!resultado.Exitoso)
            {
                ModelState.AddModelError("", resultado.Mensaje);

                await CargarCombos();

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
            var model =
                await _llamadaService.ObtenerViewModelEditar(id);

            if (model == null)
                return NotFound();

            await CargarCombos();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            LlamadaEmergenciaViewModel model)
        {
            if (id != model.IdLlamada)
                return NotFound();

            if (!ModelState.IsValid)
            {
                await CargarCombos();

                return View(model);
            }

            var resultado =
                await _llamadaService.Actualizar(model);

            if (!resultado.Exitoso)
            {
                ModelState.AddModelError("", resultado.Mensaje);

                await CargarCombos();

                return View(model);
            }

            TempData["Success"] = resultado.Mensaje;

            return RedirectToAction(nameof(Index));
        }

        //=========================================
        // DELETE
        //=========================================





        //=========================================
        // CAMBIAR ESTADO
        //=========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(int id, string estado)
        {
            var resultado = await _llamadaService.CambiarEstado(id, estado);

            TempData["Success"] = resultado.Mensaje;

            return RedirectToAction(nameof(Index));
        }

        //=========================================
        // CARGAR COMBOS
        //=========================================

        private async Task CargarCombos()
        {
            ViewBag.TiposIncidentes =
                new SelectList(
                    await _tipoIncidenteService.ObtenerTodos(),
                    "IdTipoIncidente",
                    "Nombre");
        }
    }
}