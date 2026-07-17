using SIGEBOM.Datos.Models;
using SIGEBOM.Negocio.DTOs;
using SIGEBOM.Negocio.ViewModels;

namespace SIGEBOM.Negocio.Interfaces
{
    public interface IProgramacionTurnoService
    {


        Task<List<ProgramacionTurno>> ObtenerTodos(DateOnly? fecha);

        Task<ProgramacionTurno?> ObtenerPorId(int id);

        Task<ResultadoOperacion> Crear(
            ProgramacionTurno programacion,
            List<int> bomberos);

        Task<ResultadoOperacion> Actualizar(
            ProgramacionTurno programacion,
            List<int> bomberos);

        Task<ResultadoOperacion> Desactivar(int id);

        Task<ProgramacionTurnoViewModel?> ObtenerViewModelEditar(int id);
        Task<ProgramacionTurnoDetailsViewModel?> ObtenerDetalle(int id);
    }
}