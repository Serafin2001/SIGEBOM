using Microsoft.EntityFrameworkCore;
using SIGEBOM.Datos;
using SIGEBOM.Datos.Context;
using SIGEBOM.Datos.Models;
using SIGEBOM.Negocio.DTOs;
using SIGEBOM.Negocio.Interfaces;
using SIGEBOM.Negocio.ViewModels;

namespace SIGEBOM.Negocio.Servicios
{
    public class BomberoIncidenteService : IBomberoIncidenteService
    {
        private readonly ApplicationDbContext _context;

        public BomberoIncidenteService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<BomberoIncidente>> ObtenerTodos()
        {
            return await _context.BomberosIncidentes
                .Include(x => x.Bombero)
                .Include(x => x.Incidente)
                .OrderByDescending(x => x.IdBomberoIncidente)
                .ToListAsync();
        }
        public async Task<List<BomberoIncidente>> ObtenerPorIncidente(int idIncidente)
        {
            return await _context.BomberosIncidentes
                .Include(x => x.Bombero)
                .Where(x => x.IdIncidente == idIncidente)
                .OrderBy(x => x.Bombero.Nombre)
                .ToListAsync();
        }

        public async Task<BomberoIncidente?> ObtenerPorId(int id)
        {
            return await _context.BomberosIncidentes
                .Include(x => x.Bombero)
                .Include(x => x.Incidente)
                .FirstOrDefaultAsync(x => x.IdBomberoIncidente == id);
        }
        public async Task<BomberoIncidenteViewModel?> ObtenerViewModelEditar(int id)
        {
            var registro = await _context.BomberosIncidentes
                .Include(x => x.Bombero)
                .FirstOrDefaultAsync(x => x.IdBomberoIncidente == id);

            if (registro == null)
                return null;

            return new BomberoIncidenteViewModel
            {
                IdBomberoIncidente = registro.IdBomberoIncidente,
                IdIncidente = registro.IdIncidente,
                IdBombero = registro.IdBombero,
                NombreBombero = $"{registro.Bombero.Nombre} {registro.Bombero.Apellido}",
                Funcion = registro.Funcion ?? "",
                Observacion = registro.Observacion,
                Estado = registro.Estado
            };
        }
        public async Task<ResultadoOperacion> Crear(BomberoIncidenteViewModel model)
        {
            try
            {
                // Validar que exista el incidente
                var incidente = await _context.Incidentes
                    .FirstOrDefaultAsync(x => x.IdIncidente == model.IdIncidente);

                if (incidente == null)
                {
                    return new ResultadoOperacion
                    {
                        Exitoso = false,
                        Mensaje = "El incidente seleccionado no existe."
                    };
                }

                // Validar que exista el bombero
                var bombero = await _context.Bomberos
                    .FirstOrDefaultAsync(x => x.IdBombero == model.IdBombero);

                if (bombero == null)
                {
                    return new ResultadoOperacion
                    {
                        Exitoso = false,
                        Mensaje = "El bombero seleccionado no existe."
                    };
                }

                // Validar que el bombero esté activo
                if (bombero.Estado != "Activo")
                {
                    return new ResultadoOperacion
                    {
                        Exitoso = false,
                        Mensaje = "Solo se pueden asignar bomberos activos."
                    };
                }

                // Validar que no esté asignado al mismo incidente
                bool existe = await _context.BomberosIncidentes
                    .AnyAsync(x => x.IdIncidente == model.IdIncidente
                                && x.IdBombero == model.IdBombero
                                && x.Estado == "Activo");

                if (existe)
                {
                    return new ResultadoOperacion
                    {
                        Exitoso = false,
                        Mensaje = "El bombero ya se encuentra asignado a este incidente."
                    };
                }

                var registro = new BomberoIncidente
                {
                    IdIncidente = model.IdIncidente,
                    IdBombero = model.IdBombero,
                    Funcion = model.Funcion,
                    Observacion = model.Observacion,
                    Estado = "Activo"
                };

                _context.BomberosIncidentes.Add(registro);

                await _context.SaveChangesAsync();

                return new ResultadoOperacion
                {
                    Exitoso = true,
                    Mensaje = "Bombero asignado correctamente al incidente."
                };
            }
            catch (Exception ex)
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = $"Ocurrió un error: {ex.Message}"
                };
            }
        }

        public async Task<ResultadoOperacion> Actualizar(BomberoIncidenteViewModel model)
        {
            try
            {
                var registro = await _context.BomberosIncidentes
                    .FirstOrDefaultAsync(x => x.IdBomberoIncidente == model.IdBomberoIncidente);

                if (registro == null)
                {
                    return new ResultadoOperacion
                    {
                        Exitoso = false,
                        Mensaje = "La asignación no fue encontrada."
                    };
                }

                registro.Funcion = model.Funcion;
                registro.Observacion = model.Observacion;

                await _context.SaveChangesAsync();

                return new ResultadoOperacion
                {
                    Exitoso = true,
                    Mensaje = "La asignación fue actualizada correctamente."
                };
            }
            catch (Exception ex)
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = $"Ocurrió un error: {ex.Message}"
                };
            }
        }

        public async Task<ResultadoOperacion> CambiarEstado(int id)
        {
            try
            {
                var registro = await _context.BomberosIncidentes
                    .FirstOrDefaultAsync(x => x.IdBomberoIncidente == id);

                if (registro == null)
                {
                    return new ResultadoOperacion
                    {
                        Exitoso = false,
                        Mensaje = "La asignación no fue encontrada."
                    };
                }

                registro.Estado = registro.Estado == "Activo"
                    ? "Inactivo"
                    : "Activo";

                await _context.SaveChangesAsync();

                return new ResultadoOperacion
                {
                    Exitoso = true,
                    Mensaje = $"La asignación fue {(registro.Estado == "Activo" ? "activada" : "desactivada")} correctamente."
                };
            }
            catch (Exception ex)
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = $"Ocurrió un error: {ex.Message}"
                };
            }
        }

        public async Task<List<BomberoBusquedaViewModel>> BuscarBomberos(string filtro)
        {
            return await _context.Bomberos
                .Include(x => x.Cargo)
                .Where(x =>
                    x.Estado == "Activo" &&
                    (
                        string.IsNullOrEmpty(filtro) ||

                        x.Cedula.Contains(filtro) ||

                        x.Nombre.Contains(filtro) ||

                        x.Apellido.Contains(filtro)
                    ))
                .OrderBy(x => x.Nombre)
                .Select(x => new BomberoBusquedaViewModel
                {
                    IdBombero = x.IdBombero,
                    Cedula = x.Cedula,
                    Nombre = x.Nombre,
                    Apellido = x.Apellido,
                    Cargo = x.Cargo.NombreCargo
                })
                .ToListAsync();
        }

        public async Task<Incidente?> ObtenerIncidente(int idIncidente)
        {
            return await _context.Incidentes
                .Include(i => i.TipoIncidente)
                .FirstOrDefaultAsync(i => i.IdIncidente == idIncidente);
        }

        public async Task<BomberoIncidenteIndexViewModel?> ObtenerPantalla(int idIncidente)
        {
            var incidente = await _context.Incidentes
                .Include(i => i.TipoIncidente)
                .FirstOrDefaultAsync(i => i.IdIncidente == idIncidente);

            if (incidente == null)
                return null;

            var bomberos = await _context.BomberosIncidentes
                .Include(b => b.Bombero)
                .Where(b => b.IdIncidente == idIncidente)
                .OrderBy(b => b.Bombero.Nombre)
                .ToListAsync();

            return new BomberoIncidenteIndexViewModel
            {
                Incidente = incidente,

                Bomberos = bomberos,

                NuevoBombero = new BomberoIncidenteViewModel
                {
                    IdIncidente = idIncidente
                }
            };
        }
    }
}