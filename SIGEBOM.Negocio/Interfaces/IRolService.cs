using SIGEBOM.Datos.Models;
using SIGEBOM.Negocio.DTOs;

namespace SIGEBOM.Negocio.Interfaces
{
    public interface IRolService
    {
        Task<List<Rol>> ObtenerTodos(string? buscar);

        Task<Rol?> ObtenerPorId(int id);

        Task<ResultadoOperacion> Crear(Rol rol);

        Task<ResultadoOperacion> Actualizar(Rol rol);

        Task<ResultadoOperacion> Desactivar(int id);

        Task<bool> ExisteNombre(string nombreRol);
    }
}