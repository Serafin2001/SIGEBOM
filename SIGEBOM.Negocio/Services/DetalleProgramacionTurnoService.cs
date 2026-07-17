using Microsoft.EntityFrameworkCore;
using SIGEBOM.Datos;
using SIGEBOM.Datos.Context;
using SIGEBOM.Datos.Models;
using SIGEBOM.Negocio.Interfaces;

namespace SIGEBOM.Negocio.Servicios
{
    public class DetalleProgramacionTurnoService : IDetalleProgramacionTurnoService
    {
        private readonly ApplicationDbContext _context;

        public DetalleProgramacionTurnoService(ApplicationDbContext context)
        {
            _context = context;
        }

        //=========================================
        // OBTENER BOMBEROS DE UNA PROGRAMACIÓN
        //=========================================

        public async Task<List<DetalleProgramacionTurno>> ObtenerPorProgramacion(int idProgramacion)
        {
            return await _context.DetalleProgramacionTurnos
                .Include(d => d.Bombero)
                    .ThenInclude(b => b.Rango)
                .Include(d => d.Bombero)
                    .ThenInclude(b => b.Cargo)
                .Where(d => d.IdProgramacionTurno == idProgramacion)
                .OrderBy(d => d.Bombero.Apellido)
                .ThenBy(d => d.Bombero.Nombre)
                .ToListAsync();
        }

        //=========================================
        // AGREGAR BOMBEROS
        //=========================================

        public async Task AgregarBomberos(
            int idProgramacion,
            List<int> bomberos)
        {
            foreach (var idBombero in bomberos.Distinct())
            {
                var detalle = new DetalleProgramacionTurno
                {
                    IdProgramacionTurno = idProgramacion,
                    IdBombero = idBombero,
                    Estado = "Asignado"
                };

                _context.DetalleProgramacionTurnos.Add(detalle);
            }

            await _context.SaveChangesAsync();
        }

        //=========================================
        // ACTUALIZAR BOMBEROS
        //=========================================

        public async Task ActualizarBomberos(
            int idProgramacion,
            List<int> bomberos)
        {
            var detalles = await _context.DetalleProgramacionTurnos
                .Where(d => d.IdProgramacionTurno == idProgramacion)
                .ToListAsync();

            _context.DetalleProgramacionTurnos.RemoveRange(detalles);

            await _context.SaveChangesAsync();

            await AgregarBomberos(idProgramacion, bomberos);
        }

        //=========================================
        // ELIMINAR TODOS
        //=========================================

        public async Task EliminarTodos(int idProgramacion)
        {
            var detalles = await _context.DetalleProgramacionTurnos
                .Where(d => d.IdProgramacionTurno == idProgramacion)
                .ToListAsync();

            _context.DetalleProgramacionTurnos.RemoveRange(detalles);

            await _context.SaveChangesAsync();
        }

        //=========================================
        // VALIDAR SI EL BOMBERO YA ESTÁ PROGRAMADO
        //=========================================

        public async Task<bool> BomberoYaProgramado(
            int idBombero,
            DateOnly fecha,
            int idTurno,
            int? idProgramacionExcluir = null)
        {
            var consulta = _context.DetalleProgramacionTurnos
                .Include(d => d.ProgramacionTurno)
                .Where(d =>
                    d.IdBombero == idBombero &&
                    d.ProgramacionTurno.Fecha == fecha &&
                    d.ProgramacionTurno.IdTurno == idTurno &&
                    d.ProgramacionTurno.Estado == "Programado");

            if (idProgramacionExcluir.HasValue)
            {
                consulta = consulta.Where(d =>
                    d.IdProgramacionTurno != idProgramacionExcluir.Value);
            }

            return await consulta.AnyAsync();
        }
    }
}