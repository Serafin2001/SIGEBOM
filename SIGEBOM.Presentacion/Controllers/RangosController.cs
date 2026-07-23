using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBOM.Datos.Models;
using SIGEBOM.Negocio.Interfaces;

namespace SIGEBOM.Presentacion.Controllers
{
    //[Authorize(Roles = "Administrador")]
    public class RangosController : Controller
    {
        private readonly IRangoService _rangoService;

        public RangosController(IRangoService rangoService)
        {
            _rangoService = rangoService;
        }

    
        // LISTADO
      

        public async Task<IActionResult> Index(string? buscar)
        {
            ViewData["Buscar"] = buscar;

            var rangos = await _rangoService.ObtenerTodos(buscar);

            return View(rangos);
        }

  
        // DETALLES


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var rango = await _rangoService.ObtenerPorId(id.Value);

            if (rango == null)
                return NotFound();

            return View(rango);
        }

        //=========================
        // CREAR (GET)
        //=========================

        public IActionResult Create()
        {
            return View();
        }

       
        // CREAR (POST)


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Rango rango)
        {
            if (!ModelState.IsValid)
                return View(rango);

            var resultado = await _rangoService.Crear(rango);

            if (!resultado.Exitoso)
            {
                ModelState.AddModelError("", resultado.Mensaje);
                return View(rango);
            }

            TempData["Success"] = resultado.Mensaje;

            return RedirectToAction(nameof(Index));
        }


        // EDITAR (GET)
     

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var rango = await _rangoService.ObtenerPorId(id.Value);

            if (rango == null)
                return NotFound();

            return View(rango);
        }

       
        // EDITAR (POST)

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Rango rango)
        {
            if (id != rango.IdRango)
                return NotFound();

            if (!ModelState.IsValid)
                return View(rango);

            var resultado = await _rangoService.Actualizar(rango);

            if (!resultado.Exitoso)
            {
                ModelState.AddModelError("", resultado.Mensaje);
                return View(rango);
            }

            TempData["Success"] = resultado.Mensaje;

            return RedirectToAction(nameof(Index));
        }


        // DESACTIVAR (GET)
     

        public async Task<IActionResult> Desactivar(int? id)
        {
            if (id == null)
                return NotFound();

            var rango = await _rangoService.ObtenerPorId(id.Value);

            if (rango == null)
                return NotFound();

            return View(rango);
        }

   
        // DESACTIVAR (POST)
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Desactivar(int id)
        {
            var resultado = await _rangoService.Desactivar(id);

            TempData[resultado.Exitoso ? "Success" : "Error"] = resultado.Mensaje;

            return RedirectToAction(nameof(Index));
        }
    }
}