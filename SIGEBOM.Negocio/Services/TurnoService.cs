using Microsoft.EntityFrameworkCore;
using SIGEBOM.Datos.Context;
using SIGEBOM.Datos.Models;
using SIGEBOM.Negocio.DTOs;
using SIGEBOM.Negocio.Interfaces;

namespace SIGEBOM.Negocio.Services
{
    public class TurnoService : ITurnoService
    {
        private readonly ApplicationDbContext _context;

        public TurnoService(ApplicationDbContext context)
        {
            _context = context;
        }

        //=========================================
        // OBTENER TODOS
        //=========================================

        public async Task<List<Turno>> ObtenerTodos(string? buscar)
        {
            var consulta = _context.Turnos
                .Where(t => t.Estado == "Activo")
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                consulta = consulta.Where(t =>
                    t.NombreTurno.Contains(buscar));
            }

            return await consulta
                .OrderBy(t => t.NombreTurno)
                .ToListAsync();
        }

        //=========================================
        // OBTENER POR ID
        //=========================================

        public async Task<Turno?> ObtenerPorId(int id)
        {
            return await _context.Turnos
                .FirstOrDefaultAsync(t => t.IdTurno == id);
        }

        //=========================================
        // EXISTE NOMBRE
        //=========================================

        public async Task<bool> ExisteNombre(string nombre)
        {
            return await _context.Turnos
                .AnyAsync(t =>
                    t.NombreTurno == nombre &&
                    t.Estado == "Activo");
        }

        //=========================================
        // CREAR
        //=========================================

        public async Task<ResultadoOperacion> Crear(Turno turno)
        {
            if (await ExisteNombre(turno.NombreTurno))
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = "Ya existe un turno con ese nombre."
                };
            }

            turno.Estado = "Activo";

            _context.Turnos.Add(turno);

            await _context.SaveChangesAsync();

            return new ResultadoOperacion
            {
                Exitoso = true,
                Mensaje = "Turno registrado correctamente."
            };
        }

        //=========================================
        // ACTUALIZAR
        //=========================================

        public async Task<ResultadoOperacion> Actualizar(Turno turno)
        {
            var turnoBD = await _context.Turnos
                .FirstOrDefaultAsync(t => t.IdTurno == turno.IdTurno);

            if (turnoBD == null)
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = "El turno no existe."
                };
            }

            bool existe = await _context.Turnos.AnyAsync(t =>
                t.IdTurno != turno.IdTurno &&
                t.NombreTurno == turno.NombreTurno &&
                t.Estado == "Activo");

            if (existe)
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = "Ya existe otro turno con ese nombre."
                };
            }

            turnoBD.NombreTurno = turno.NombreTurno;
            turnoBD.HoraEntrada = turno.HoraEntrada;
            turnoBD.HoraSalida = turno.HoraSalida;
            turnoBD.Descripcion = turno.Descripcion;

            _context.Update(turnoBD);

            await _context.SaveChangesAsync();

            return new ResultadoOperacion
            {
                Exitoso = true,
                Mensaje = "Turno actualizado correctamente."
            };
        }

        //=========================================
        // DESACTIVAR
        //=========================================

        public async Task<ResultadoOperacion> Desactivar(int id)
        {
            var turno = await _context.Turnos
                .FirstOrDefaultAsync(t => t.IdTurno == id);

            if (turno == null)
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = "El turno no existe."
                };
            }

            turno.Estado = "Inactivo";

            _context.Update(turno);

            await _context.SaveChangesAsync();

            return new ResultadoOperacion
            {
                Exitoso = true,
                Mensaje = "Turno desactivado correctamente."
            };
        }
    }
}