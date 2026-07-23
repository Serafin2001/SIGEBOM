using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBOM.Datos.Models;
using SIGEBOM.Negocio.Interfaces;

namespace SIGEBOM.Presentacion.Controllers
{
    //[Authorize(Roles = "Administrador")]
    public class RolesController : Controller
    {
        private readonly IRolService _rolService;

        public RolesController(IRolService rolService)
        {
            _rolService = rolService;
        }

        
        // LISTADO
      

        public async Task<IActionResult> Index(string? buscar)
        {
            ViewData["Buscar"] = buscar;

            var roles = await _rolService.ObtenerTodos(buscar);

            return View(roles);
        }

        
        // DETALLES
        

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var rol = await _rolService.ObtenerPorId(id.Value);

            if (rol == null)
                return NotFound();

            return View(rol);
        }

     
        // CREAR (GET)
       

        public IActionResult Create()
        {
            return View();
        }

       
        // CREAR (POST)
 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Rol rol)
        {
            if (!ModelState.IsValid)
                return View(rol);

            var resultado = await _rolService.Crear(rol);

            if (!resultado.Exitoso)
            {
                ModelState.AddModelError("", resultado.Mensaje);
                return View(rol);
            }

            TempData["Success"] = resultado.Mensaje;

            return RedirectToAction(nameof(Index));
        }

   
        // EDITAR (GET)
     

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var rol = await _rolService.ObtenerPorId(id.Value);

            if (rol == null)
                return NotFound();

            return View(rol);
        }


        // EDITAR (POST)


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Rol rol)
        {
            if (id != rol.IdRol)
                return NotFound();

            if (!ModelState.IsValid)
                return View(rol);

            var resultado = await _rolService.Actualizar(rol);

            if (!resultado.Exitoso)
            {
                ModelState.AddModelError("", resultado.Mensaje);
                return View(rol);
            }

            TempData["Success"] = resultado.Mensaje;

            return RedirectToAction(nameof(Index));
        }


        // DESACTIVAR (GET)
      

        public async Task<IActionResult> Desactivar(int? id)
        {
            if (id == null)
                return NotFound();

            var rol = await _rolService.ObtenerPorId(id.Value);

            if (rol == null)
                return NotFound();

            return View(rol);
        }

  
        // DESACTIVAR (POST)
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Desactivar(int id)
        {
            var resultado = await _rolService.Desactivar(id);

            TempData[resultado.Exitoso ? "Success" : "Error"] = resultado.Mensaje;

            return RedirectToAction(nameof(Index));
        }
    }
}