using SIGEBOM.Datos.Models;
using SIGEBOM.Negocio.DTOs;

namespace SIGEBOM.Negocio.Interfaces
{
    public interface ICargoService
    {
        Task<List<Cargo>> ObtenerTodos(string? buscar);

        Task<Cargo?> ObtenerPorId(int id);

        Task<ResultadoOperacion> Crear(Cargo cargo);

        Task<ResultadoOperacion> Actualizar(Cargo cargo);

        Task<ResultadoOperacion> Desactivar(int id);

        Task<bool> ExisteNombre(string nombre);
    }
}