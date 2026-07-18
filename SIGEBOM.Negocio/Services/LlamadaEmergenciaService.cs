using Microsoft.EntityFrameworkCore;
using SIGEBOM.Datos.Context;
using SIGEBOM.Datos.Models;
using SIGEBOM.Negocio.DTOs;
using SIGEBOM.Negocio.Interfaces;
using SIGEBOM.Negocio.ViewModels;

namespace SIGEBOM.Negocio.Servicios
{
    public class LlamadaEmergenciaService : ILlamadaEmergenciaService
    {
        private readonly ApplicationDbContext _context;

        public LlamadaEmergenciaService(ApplicationDbContext context)
        {
            _context = context;
        }

        //=========================================
        // OBTENER TODAS
        //=========================================

        public async Task<List<LlamadaEmergencia>> ObtenerTodas()
        {
            return await _context.LlamadasEmergencia
                .Include(x => x.Usuario)
                .Include(x => x.TipoIncidente)
                .OrderByDescending(x => x.FechaHora)
                .ToListAsync();
        }

        //=========================================
        // OBTENER POR ID
        //=========================================

        public async Task<LlamadaEmergencia?> ObtenerPorId(int id)
        {
            return await _context.LlamadasEmergencia
                .Include(x => x.Usuario)
                .Include(x => x.TipoIncidente)
                .FirstOrDefaultAsync(x => x.IdLlamada == id);
        }

        //=========================================
        // OBTENER VIEWMODEL EDITAR
        //=========================================

        public async Task<LlamadaEmergenciaViewModel?> ObtenerViewModelEditar(int id)
        {
            var llamada = await _context.LlamadasEmergencia
                .FirstOrDefaultAsync(x => x.IdLlamada == id);

            if (llamada == null)
                return null;

            return new LlamadaEmergenciaViewModel
            {
                IdLlamada = llamada.IdLlamada,
                NombreReportante = llamada.NombreReportante,
                Telefono = llamada.Telefono,
                Direccion = llamada.Direccion ?? "",
                FechaHora = llamada.FechaHora,
                Observacion = llamada.Observacion,
                Estado = llamada.Estado,
                IdTipoIncidente = llamada.IdTipoIncidente
            };
        }

        //=========================================
        // CREAR
        //=========================================

        public async Task<ResultadoOperacion> Crear(
            LlamadaEmergenciaViewModel model,
            int idUsuario)
        {
            try
            {
                var llamada = new LlamadaEmergencia
                {
                    NombreReportante = model.NombreReportante,
                    Telefono = model.Telefono,
                    Direccion = model.Direccion,
                    FechaHora = DateTime.Now,
                    Observacion = model.Observacion,
                    Estado = "Pendiente",
                    IdUsuario = idUsuario,
                    IdTipoIncidente = model.IdTipoIncidente
                };

                _context.LlamadasEmergencia.Add(llamada);

                await _context.SaveChangesAsync();

                return new ResultadoOperacion
                {
                    Exitoso = true,
                    Mensaje = "Llamada registrada correctamente."
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

        public async Task<ResultadoOperacion> Actualizar(
            LlamadaEmergenciaViewModel model)
        {
            try
            {
                var llamada = await _context.LlamadasEmergencia
                    .FindAsync(model.IdLlamada);

                if (llamada == null)
                {
                    return new ResultadoOperacion
                    {
                        Exitoso = false,
                        Mensaje = "La llamada no existe."
                    };
                }

                llamada.NombreReportante = model.NombreReportante;
                llamada.Telefono = model.Telefono;
                llamada.Direccion = model.Direccion;
                llamada.Observacion = model.Observacion;
                llamada.IdTipoIncidente = model.IdTipoIncidente;

                await _context.SaveChangesAsync();

                return new ResultadoOperacion
                {
                    Exitoso = true,
                    Mensaje = "Llamada actualizada correctamente."
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
                var llamada = await _context.LlamadasEmergencia
                    .FirstOrDefaultAsync(x => x.IdLlamada == id);

                if (llamada == null)
                {
                    return new ResultadoOperacion
                    {
                        Exitoso = false,
                        Mensaje = "La llamada no existe."
                    };
                }

                llamada.Estado = estado;

                _context.LlamadasEmergencia.Update(llamada);

                await _context.SaveChangesAsync();

                return new ResultadoOperacion
                {
                    Exitoso = true,
                    Mensaje = "Estado actualizado correctamente."
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
