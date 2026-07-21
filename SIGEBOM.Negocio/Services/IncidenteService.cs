using Microsoft.EntityFrameworkCore;
using SIGEBOM.Datos.Context;
using SIGEBOM.Datos.Models;
using SIGEBOM.Negocio.DTOs;
using SIGEBOM.Negocio.Interfaces;
using SIGEBOM.Negocio.ViewModels;

namespace SIGEBOM.Negocio.Servicios
{
    public class IncidenteService : IIncidenteService
    {
        private readonly ApplicationDbContext _context;

        public IncidenteService(ApplicationDbContext context)
        {
            _context = context;
        }

    

        public async Task<List<Incidente>> ObtenerTodos()
        {
            return await _context.Incidentes
                .Include(i => i.LlamadaEmergencia)
                .Include(i => i.TipoIncidente)
                .OrderByDescending(i => i.FechaHoraIncidente)
                .ToListAsync();
        }



        public async Task<Incidente?> ObtenerPorId(int id)
        {
            return await _context.Incidentes
                .Include(i => i.LlamadaEmergencia)
                .Include(i => i.TipoIncidente)
                .FirstOrDefaultAsync(i => i.IdIncidente == id);
        }


        public async Task<IncidenteViewModel?> ObtenerViewModelEditar(int id)
        {
            var incidente = await _context.Incidentes
                .Include(i => i.TipoIncidente)
                .FirstOrDefaultAsync(i => i.IdIncidente == id);

            if (incidente == null)
                return null;

            return new IncidenteViewModel
            {
                IdIncidente = incidente.IdIncidente,
                IdLlamada = incidente.IdLlamada,
                IdTipoIncidente = incidente.IdTipoIncidente,
                NombreTipoIncidente = incidente.TipoIncidente.Nombre,
                Direccion = incidente.Direccion,
                FechaHoraIncidente = incidente.FechaHoraIncidente,
                HoraSalida = incidente.HoraSalida,
                HoraLlegada = incidente.HoraLlegada,
                HoraFinalizacion = incidente.HoraFinalizacion,
                Descripcion = incidente.Descripcion ?? string.Empty
            };
        }



        public async Task<IncidenteViewModel?> ObtenerDesdeLlamada(int idLlamada)
        {
            var llamada = await _context.LlamadasEmergencia
                .Include(x => x.TipoIncidente)
                .FirstOrDefaultAsync(x => x.IdLlamada == idLlamada);

            if (llamada == null)
                return null;

            return new IncidenteViewModel
            {
                IdLlamada = llamada.IdLlamada,
                IdTipoIncidente = llamada.IdTipoIncidente,
                NombreTipoIncidente = llamada.TipoIncidente.Nombre,
                Direccion = llamada.Direccion ?? string.Empty,
                FechaHoraIncidente = llamada.FechaHora,
                HoraSalida = DateTime.Now.TimeOfDay
            };
        }



        public async Task<ResultadoOperacion> Crear(IncidenteViewModel model)
        {
            try
            {
                bool existeIncidente = await _context.Incidentes
                    .AnyAsync(i => i.IdLlamada == model.IdLlamada);

                if (existeIncidente)
                {
                    return new ResultadoOperacion
                    {
                        Exitoso = false,
                        Mensaje = "Esta llamada ya tiene un incidente registrado."
                    };
                }

                var llamada = await _context.LlamadasEmergencia
                    .FirstOrDefaultAsync(x => x.IdLlamada == model.IdLlamada);

                if (llamada == null)
                {
                    return new ResultadoOperacion
                    {
                        Exitoso = false,
                        Mensaje = "La llamada no existe."
                    };
                }

                var incidente = new Incidente
                {
                    IdLlamada = model.IdLlamada,
                    IdTipoIncidente = model.IdTipoIncidente,
                    Direccion = model.Direccion,
                    FechaHoraIncidente = model.FechaHoraIncidente,
                    HoraSalida = model.HoraSalida!.Value,
                    HoraLlegada = model.HoraLlegada!.Value,
                    HoraFinalizacion = model.HoraFinalizacion!.Value,
                    Descripcion = model.Descripcion,
                    Estado = "Finalizado"
                };

                _context.Incidentes.Add(incidente);

                // Actualizar el estado de la llamada
                llamada.Estado = "Atendida";

                await _context.SaveChangesAsync();

                return new ResultadoOperacion
                {
                    Exitoso = true,
                    Mensaje = "Incidente registrado correctamente."
                };
            }
            catch
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = "Ocurrió un error al registrar el incidente."
                };
            }
        }

        //=========================================
        // ACTUALIZAR
        //=========================================

        public async Task<ResultadoOperacion> Actualizar(IncidenteViewModel model)
        {
            try
            {
                var incidente = await _context.Incidentes
                    .FirstOrDefaultAsync(i => i.IdIncidente == model.IdIncidente);

                if (incidente == null)
                {
                    return new ResultadoOperacion
                    {
                        Exitoso = false,
                        Mensaje = "El incidente no existe."
                    };
                }

                incidente.HoraSalida = model.HoraSalida!.Value;
                incidente.HoraLlegada = model.HoraLlegada!.Value;
                incidente.HoraFinalizacion = model.HoraFinalizacion!.Value;
                incidente.Descripcion = model.Descripcion;

                await _context.SaveChangesAsync();

                return new ResultadoOperacion
                {
                    Exitoso = true,
                    Mensaje = "Incidente actualizado correctamente."
                };
            }
            catch
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = "Ocurrió un error al actualizar el incidente."
                };
            }
        }

        //=========================================
        // VALIDAR SI LA LLAMADA YA TIENE INCIDENTE
        //=========================================

        public async Task<bool> ExisteIncidentePorLlamada(int idLlamada)
        {
            return await _context.Incidentes
                .AnyAsync(i => i.IdLlamada == idLlamada);
        }
    }
}