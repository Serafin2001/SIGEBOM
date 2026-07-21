using Microsoft.EntityFrameworkCore;
using SIGEBOM.Datos.Context;
using SIGEBOM.Datos.Models;
using SIGEBOM.Negocio.DTOs;
using SIGEBOM.Negocio.Interfaces;
using SIGEBOM.Negocio.ViewModels;

namespace SIGEBOM.Negocio.Services
{
    public class ProgramacionTurnoService : IProgramacionTurnoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDetalleProgramacionTurnoService _detalleService;

        public ProgramacionTurnoService(
       ApplicationDbContext context,
       IDetalleProgramacionTurnoService detalleService)
        {
            _context = context;
            _detalleService = detalleService;
        }


        public async Task<List<ProgramacionTurno>> ObtenerTodos(DateOnly? fecha)
        {
            var consulta = _context.ProgramacionTurnos
                .Include(p => p.Turno)
                .Include(p => p.Encargado)
                .Include(p => p.DetallesProgramacion)
                .Where(p => p.Estado != "Cancelado")
                .AsQueryable();

            if (fecha.HasValue)
            {
                consulta = consulta.Where(p => p.Fecha == fecha.Value);
            }

            return await consulta
                .OrderByDescending(p => p.Fecha)
                .ToListAsync();
        }

        public async Task<ProgramacionTurno?> ObtenerPorId(int id)
        {
            return await _context.ProgramacionTurnos
                .AsNoTracking()
                .Include(p => p.Turno)
                .Include(p => p.Encargado)
                .Include(p => p.UsuarioCreador)
                .Include(p => p.DetallesProgramacion)
                    .ThenInclude(d => d.Bombero)
                .FirstOrDefaultAsync(p => p.IdProgramacionTurno == id);
        }


        public async Task<ResultadoOperacion> Crear( ProgramacionTurno programacion,
            List<int> bomberos)
        {
            using var transaccion = await _context.Database.BeginTransactionAsync();

            try
            {
                //=========================================
                // VALIDACIONES
                //=========================================

                if (programacion.IdTurno <= 0)
                {
                    return new ResultadoOperacion
                    {
                        Exitoso = false,
                        Mensaje = "Debe seleccionar un turno."
                    };
                }

                if (programacion.IdEncargado <= 0)
                {
                    return new ResultadoOperacion
                    {
                        Exitoso = false,
                        Mensaje = "Debe seleccionar el encargado del turno."
                    };
                }

                if (bomberos == null || !bomberos.Any())
                {
                    return new ResultadoOperacion
                    {
                        Exitoso = false,
                        Mensaje = "Debe asignar al menos un bombero."
                    };
                }

                // Elimina posibles duplicados enviados desde la vista
                bomberos = bomberos.Distinct().ToList();

                // El encargado debe estar incluido dentro del personal asignado
                if (!bomberos.Contains(programacion.IdEncargado))
                {
                    return new ResultadoOperacion
                    {
                        Exitoso = false,
                        Mensaje = "El encargado del turno debe formar parte del personal asignado."
                    };
                }

                //=========================================
                // VALIDAR DUPLICADOS
                //=========================================

                foreach (var idBombero in bomberos)
                {
                    bool existe = await _context.DetalleProgramacionTurnos
                        .Include(d => d.ProgramacionTurno)
                        .AnyAsync(d =>
                            d.IdBombero == idBombero &&
                            d.ProgramacionTurno.Fecha == programacion.Fecha &&
                            d.ProgramacionTurno.IdTurno == programacion.IdTurno &&
                            d.ProgramacionTurno.Estado != "Cancelado");

                    if (existe)
                    {
                        var bombero = await _context.Bomberos
                            .FirstOrDefaultAsync(b => b.IdBombero == idBombero);

                        return new ResultadoOperacion
                        {
                            Exitoso = false,
                            Mensaje = $"El bombero {bombero?.NombreCompleto} ya está asignado a este turno en la fecha seleccionada."
                        };
                    }
                }

                //=========================================
                // CREAR PROGRAMACIÓN
                //=========================================

                programacion.FechaCreacion = DateTime.Now;
                programacion.Estado = "Programado";

                _context.ProgramacionTurnos.Add(programacion);

                await _context.SaveChangesAsync();

                foreach (var idBombero in bomberos)
                {
                    bool existe = await _detalleService.BomberoYaProgramado(
                        idBombero,
                        programacion.Fecha,
                        programacion.IdTurno);

                    if (existe)
                    {
                        return new ResultadoOperacion
                        {
                            Exitoso = false,
                            Mensaje = "Uno o más bomberos ya están asignados a este turno en la fecha seleccionada."
                        };
                    }
                }

                //=========================================
                // DETALLE
                //=========================================

                foreach (var idBombero in bomberos)
                {
                    _context.DetalleProgramacionTurnos.Add(
                        new DetalleProgramacionTurno
                        {
                            IdProgramacionTurno = programacion.IdProgramacionTurno,
                            IdBombero = idBombero,
                            Estado = "Asignado",
                            Observacion = null
                        });
                }

                await _context.SaveChangesAsync();

                await transaccion.CommitAsync();

                return new ResultadoOperacion
                {
                    Exitoso = true,
                    Mensaje = "Programación registrada correctamente."
                };
            }
            catch (Exception ex)
            {
                await transaccion.RollbackAsync();

                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = $"Ocurrió un error al registrar la programación. {ex.Message}"
                };
            }
        }

        public async Task<ResultadoOperacion> Actualizar(
            ProgramacionTurno programacion,
            List<int> bomberos)
        {
            using var transaccion = await _context.Database.BeginTransactionAsync();

            try
            {
                var programacionBD = await _context.ProgramacionTurnos
                    .Include(p => p.DetallesProgramacion)
                    .FirstOrDefaultAsync(p =>
                        p.IdProgramacionTurno == programacion.IdProgramacionTurno);

                if (programacionBD == null)
                {
                    return new ResultadoOperacion
                    {
                        Exitoso = false,
                        Mensaje = "La programación no existe."
                    };
                }

                programacionBD.Fecha = programacion.Fecha;
                programacionBD.IdTurno = programacion.IdTurno;
                programacionBD.IdEncargado = programacion.IdEncargado;
                programacionBD.Observacion = programacion.Observacion;

                _context.DetalleProgramacionTurnos.RemoveRange(
                    programacionBD.DetallesProgramacion);

                await _context.SaveChangesAsync();
                await _detalleService.ActualizarBomberos(
                programacion.IdProgramacionTurno,
                bomberos);

                foreach (var idBombero in bomberos)
                {
                    bool existe = await _detalleService.BomberoYaProgramado(
                        idBombero,
                        programacion.Fecha,
                        programacion.IdTurno,
                        programacion.IdProgramacionTurno);

                    if (existe)
                    {
                        return new ResultadoOperacion
                        {
                            Exitoso = false,
                            Mensaje = "Uno o más bomberos ya están asignados a este turno en la fecha seleccionada."
                        };
                    }
                }

                await _context.SaveChangesAsync();

                await transaccion.CommitAsync();

                return new ResultadoOperacion
                {
                    Exitoso = true,
                    Mensaje = "Programación actualizada correctamente."
                };
            }
            catch
            {
                await transaccion.RollbackAsync();

                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = "Ocurrió un error al actualizar la programación."
                };
            }
        }



        public async Task<ResultadoOperacion> Desactivar(int id)
        {
            var programacion = await _context.ProgramacionTurnos
                .FirstOrDefaultAsync(p =>
                    p.IdProgramacionTurno == id);

            if (programacion == null)
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = "La programación no existe."
                };
            }

            programacion.Estado = "Cancelado";
            await _detalleService.EliminarTodos(id);

            _context.Update(programacion);

            await _context.SaveChangesAsync();

            return new ResultadoOperacion
            {
                Exitoso = true,
                Mensaje = "Programación cancelada correctamente."
            };
        }

        public async Task<ProgramacionTurnoViewModel?> ObtenerViewModelEditar(int id)
        {
            var programacion = await _context.ProgramacionTurnos
                .Include(p => p.Turno)
                .Include(p => p.Encargado)
                .Include(p => p.DetallesProgramacion)
                    .ThenInclude(d => d.Bombero)
                .FirstOrDefaultAsync(p => p.IdProgramacionTurno == id);

            if (programacion == null)
                return null;

            var turnos = await _context.Turnos
                .Where(t => t.Estado == "Activo")
                .OrderBy(t => t.NombreTurno)
                .ToListAsync();

            var bomberos = programacion.DetallesProgramacion
                .Select(d => d.Bombero)
                .OrderBy(b => b.Nombre)
                .ThenBy(b => b.Apellido)
                .ToList();

            return new ProgramacionTurnoViewModel
            {
                IdProgramacionTurno = programacion.IdProgramacionTurno,
                Fecha = programacion.Fecha,
                IdTurno = programacion.IdTurno,
                IdEncargado = programacion.IdEncargado,
                Observacion = programacion.Observacion,
                Estado = programacion.Estado,

                CedulaEncargado = programacion.Encargado.Cedula,
                NombreEncargado = programacion.Encargado.NombreCompleto,

                Turnos = turnos,
                Bomberos = bomberos,
                BomberosSeleccionados = bomberos
                    .Select(b => b.IdBombero)
                    .ToList()
            };
        }

        public async Task<ProgramacionTurnoDetailsViewModel?> ObtenerDetalle(int id)
        {
            var programacion = await _context.ProgramacionTurnos
                .Include(p => p.Turno)
                .Include(p => p.Encargado)
                .FirstOrDefaultAsync(p => p.IdProgramacionTurno == id);

            if (programacion == null)
                return null;

            var bomberos = await _detalleService
                .ObtenerPorProgramacion(id);

            return new ProgramacionTurnoDetailsViewModel
            {
                Programacion = programacion,
                Bomberos = bomberos
            };
        }

        public async Task<ResultadoOperacion> GenerarSemana(DateTime fechaInicio)
        {
            try
            {
                // Validar que la fecha seleccionada sea lunes
                if (fechaInicio.DayOfWeek != DayOfWeek.Monday)
                {
                    return new ResultadoOperacion
                    {
                        Exitoso = false,
                        Mensaje = "La fecha seleccionada debe ser un lunes."
                    };
                }

                DateOnly fechaInicioOnly = DateOnly.FromDateTime(fechaInicio);
                DateOnly fechaFinOnly = DateOnly.FromDateTime(fechaInicio.AddDays(6));

                bool existe = await _context.ProgramacionTurnos
                    .AnyAsync(x => x.Fecha >= fechaInicioOnly &&
                                   x.Fecha <= fechaFinOnly);

                if (existe)
                {
                    return new ResultadoOperacion
                    {
                        Exitoso = false,
                        Mensaje = "Ya existe una programación para esa semana."
                    };
                }

                // Obtener todos los turnos activos
                var turnos = await _context.Turnos
                    .Where(x => x.Estado == "Activo")
                    .OrderBy(x => x.IdTurno)
                    .ToListAsync();

                if (!turnos.Any())
                {
                    return new ResultadoOperacion
                    {
                        Exitoso = false,
                        Mensaje = "No existen turnos activos."
                    };
                }

                var programaciones = new List<ProgramacionTurno>();

                // Crear las programaciones de los 7 días
                for (int dia = 0; dia < 7; dia++)
                {
                    DateOnly fecha = DateOnly.FromDateTime(fechaInicio.AddDays(dia));

                    foreach (var turno in turnos)
                    {

                        programaciones.Add(new ProgramacionTurno
                        {
                            Fecha = fecha,
                            IdTurno = turno.IdTurno,
                            Estado = "Programado"
                        });
                    }
                }

                await _context.ProgramacionTurnos.AddRangeAsync(programaciones);
                await _context.SaveChangesAsync();

                return new ResultadoOperacion
                {
                    Exitoso = true,
                    Mensaje = $"Se generaron {programaciones.Count} programaciones correctamente."
                };
            }
            catch (Exception ex)
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = ex.InnerException?.Message ?? ex.Message
                };
            }
        }

        public async Task<ResultadoOperacion> CambiarEstado(int id)
        {
            var programacion = await _context.ProgramacionTurnos
                .FirstOrDefaultAsync(x => x.IdProgramacionTurno == id);

            if (programacion == null)
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = "La programación no existe."
                };
            }

            if (programacion.Estado == "Programado")
            {
                programacion.Estado = "En Curso";
            }
            else if (programacion.Estado == "En Curso")
            {
                programacion.Estado = "Finalizado";
            }
            else
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = "La programación ya no puede cambiar de estado."
                };
            }

            await _context.SaveChangesAsync();

            return new ResultadoOperacion
            {
                Exitoso = true,
                Mensaje = "Estado actualizado correctamente."
            };
        }
    }
}