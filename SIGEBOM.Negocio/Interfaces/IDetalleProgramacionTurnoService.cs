using SIGEBOM.Datos.Models;

namespace SIGEBOM.Negocio.Interfaces
{
    public interface IDetalleProgramacionTurnoService
    {
        Task<List<DetalleProgramacionTurno>> ObtenerPorProgramacion(int idProgramacion);

        Task AgregarBomberos(
            int idProgramacion,
            List<int> bomberos);

        Task ActualizarBomberos(
            int idProgramacion,
            List<int> bomberos);

        Task EliminarTodos(int idProgramacion);
        Task<bool> BomberoYaProgramado(
    int idBombero,
    DateOnly fecha,
    int idTurno,
    int? idProgramacionExcluir = null);
    }
}