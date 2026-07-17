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

        public ProgramacionTurnoService(ApplicationDbContext context)
        {
            _context = context;
        }

        //=========================================
        // OBTENER TODAS LAS PROGRAMACIONES
        //=========================================

        public async Task<List<ProgramacionTurno>> ObtenerTodos(DateOnly? fecha)
        {
            var consulta = _context.ProgramacionTurnos
                .Include(p => p.Turno)
                .Include(p => p.Encargado)
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


        public async Task<ResultadoOperacion> Crear(
            ProgramacionTurno programacion,
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

        //=========================================
        // ACTUALIZAR
        //=========================================

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

                foreach (var idBombero in bomberos)
                {
                    _context.DetalleProgramacionTurnos.Add(
                        new DetalleProgramacionTurno
                        {
                            IdProgramacionTurno = programacionBD.IdProgramacionTurno,
                            IdBombero = idBombero,
                            Estado = "Asignado"
                        });
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

        //=========================================
        // DESACTIVAR
        //=========================================

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
    }
}