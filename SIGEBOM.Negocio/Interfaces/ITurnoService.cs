using SIGEBOM.Datos.Models;
using SIGEBOM.Negocio.DTOs;

namespace SIGEBOM.Negocio.Interfaces
{
    public interface ITurnoService
    {
        Task<List<Turno>> ObtenerTodos(string? buscar);

        Task<Turno?> ObtenerPorId(int id);

        Task<ResultadoOperacion> Crear(Turno turno);

        Task<ResultadoOperacion> Actualizar(Turno turno);

        Task<ResultadoOperacion> Desactivar(int id);

        Task<bool> ExisteNombre(string nombre);
    }
}