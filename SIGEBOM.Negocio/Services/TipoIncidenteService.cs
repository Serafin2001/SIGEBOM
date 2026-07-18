using Microsoft.EntityFrameworkCore;
using SIGEBOM.Datos.Context;
using SIGEBOM.Datos.Models;
using SIGEBOM.Negocio.DTOs;
using SIGEBOM.Negocio.Interfaces;
using SIGEBOM.Negocio.ViewModels;

namespace SIGEBOM.Negocio.Servicios
{
    public class TipoIncidenteService : ITipoIncidenteService
    {
        private readonly ApplicationDbContext _context;

        public TipoIncidenteService(ApplicationDbContext context)
        {
            _context = context;
        }

        //=========================================
        // OBTENER TODOS
        //=========================================

        public async Task<List<TipoIncidente>> ObtenerTodos()
        {
            return await _context.TiposIncidentes
                .OrderBy(t => t.Nombre)
                .ToListAsync();
        }

        //=========================================
        // OBTENER POR ID
        //=========================================

        public async Task<TipoIncidente?> ObtenerPorId(int id)
        {
            return await _context.TiposIncidentes
                .FirstOrDefaultAsync(t => t.IdTipoIncidente == id);
        }

        //=========================================
        // OBTENER VIEWMODEL PARA EDITAR
        //=========================================

        public async Task<TipoIncidenteViewModel?> ObtenerViewModelEditar(int id)
        {
            var tipo = await _context.TiposIncidentes
                .FirstOrDefaultAsync(t => t.IdTipoIncidente == id);

            if (tipo == null)
                return null;

            return new TipoIncidenteViewModel
            {
                IdTipoIncidente = tipo.IdTipoIncidente,
                Nombre = tipo.Nombre,
                Descripcion = tipo.Descripcion,
                Estado = tipo.Estado,
                Codigo = tipo.Codigo
            };
        }

        //=========================================
        // CREAR
        //=========================================

        public async Task<ResultadoOperacion> Crear(TipoIncidenteViewModel model)
        {
            try
            {
                bool existe = await _context.TiposIncidentes
                    .AnyAsync(t => t.Nombre.ToLower() == model.Nombre.ToLower());
                
                var existeCodigo = await _context.TiposIncidentes
              .AnyAsync(t => t.Codigo == model.Codigo);

                if (existeCodigo)
                {
                    return new ResultadoOperacion
                    {
                        Exitoso = false,
                        Mensaje = "Ya existe un tipo de incidente con ese código."
                    };
                }
                if (existe)
                {
                    return new ResultadoOperacion
                    {
                        Exitoso = false,
                        Mensaje = "Ya existe un tipo de incidente con ese nombre."
                    };
                }

                var tipo = new TipoIncidente
                {
                    Nombre = model.Nombre.Trim(),
                    Descripcion = model.Descripcion,
                    Codigo = model.Codigo,
                    Estado = "Activo"
                };

                _context.TiposIncidentes.Add(tipo);

                await _context.SaveChangesAsync();

                return new ResultadoOperacion
                {
                    Exitoso = true,
                    Mensaje = "Tipo de incidente registrado correctamente."
                };
            }
            catch (Exception ex)
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = ex.Message
                };
            }
        }

        //=========================================
        // ACTUALIZAR
        //=========================================

        public async Task<ResultadoOperacion> Actualizar(TipoIncidenteViewModel model)
        {
            try
            {
                var tipo = await _context.TiposIncidentes
                    .FirstOrDefaultAsync(t => t.IdTipoIncidente == model.IdTipoIncidente);
                var existeCodigo = await _context.TiposIncidentes
                    .AnyAsync(t => t.Codigo == model.Codigo &&
                   t.IdTipoIncidente != model.IdTipoIncidente);

                if (existeCodigo)
                {
                    return new ResultadoOperacion
                    {
                        Exitoso = false,
                        Mensaje = "Ya existe un tipo de incidente con ese código."
                    };
                }
                if (tipo == null)
                {
                    return new ResultadoOperacion
                    {
                        Exitoso = false,
                        Mensaje = "El tipo de incidente no existe."
                    };
                }

                bool existe = await _context.TiposIncidentes
                    .AnyAsync(t =>
                        t.IdTipoIncidente != model.IdTipoIncidente &&
                        t.Nombre.ToLower() == model.Nombre.ToLower());
                        

                if (existe)
                {
                    return new ResultadoOperacion
                    {
                        Exitoso = false,
                        Mensaje = "Ya existe un tipo de incidente con ese nombre."
                    };
                }

                tipo.Codigo = model.Codigo;
                tipo.Nombre = model.Nombre;
                tipo.Descripcion = model.Descripcion;

                await _context.SaveChangesAsync();

                return new ResultadoOperacion
                {
                    Exitoso = true,
                    Mensaje = "Tipo de incidente actualizado correctamente."
                };
            }
            catch (Exception ex)
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = ex.Message
                };
            }
        }

        //=========================================
        // CAMBIAR ESTADO
        //=========================================

        public async Task<ResultadoOperacion> CambiarEstado(int id, string estado)
        {
            try
            {
                var tipo = await _context.TiposIncidentes
                    .FirstOrDefaultAsync(t => t.IdTipoIncidente == id);

                if (tipo == null)
                {
                    return new ResultadoOperacion
                    {
                        Exitoso = false,
                        Mensaje = "El tipo de incidente no existe."
                    };
                }

                tipo.Estado = estado;

                await _context.SaveChangesAsync();

                return new ResultadoOperacion
                {
                    Exitoso = true,
                    Mensaje = $"Estado cambiado a {estado} correctamente."
                };
            }
            catch (Exception ex)
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = ex.Message
                };
            }
        }
    }
}
