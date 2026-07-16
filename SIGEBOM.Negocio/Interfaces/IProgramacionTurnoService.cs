using SIGEBOM.Datos.Models;
using SIGEBOM.Negocio.DTOs;

namespace SIGEBOM.Negocio.Interfaces
{
    public interface IProgramacionTurnoService
    {
        Task<List<ProgramacionTurno>> ObtenerTodos(DateTime? fecha);

        Task<ProgramacionTurno?> ObtenerPorId(int id);

        Task<ResultadoOperacion> Crear(ProgramacionTurno programacion, List<int> bomberos);

        Task<ResultadoOperacion> Actualizar(ProgramacionTurno programacion, List<int> bomberos);

        Task<ResultadoOperacion> Desactivar(int id);
    }
}