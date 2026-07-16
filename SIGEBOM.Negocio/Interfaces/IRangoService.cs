using SIGEBOM.Datos.Models;
using SIGEBOM.Negocio.DTOs;

namespace SIGEBOM.Negocio.Interfaces
{
    public interface IRangoService
    {
        Task<List<Rango>> ObtenerTodos(string? buscar);

        Task<Rango?> ObtenerPorId(int id);

        Task<ResultadoOperacion> Crear(Rango rango);

        Task<ResultadoOperacion> Actualizar(Rango rango);

        Task<ResultadoOperacion> Desactivar(int id);

        Task<bool> ExisteNombre(string nombreRango);
    }
}