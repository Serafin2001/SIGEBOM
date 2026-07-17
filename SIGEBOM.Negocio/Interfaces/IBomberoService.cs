using SIGEBOM.Datos.Models;
using SIGEBOM.Negocio.DTOs;

namespace SIGEBOM.Negocio.Interfaces
{
    public interface IBomberoService
    {
        Task<List<Bombero>> ObtenerTodos(string? buscar);

        Task<Bombero?> ObtenerPorId(int id);

        Task<ResultadoOperacion> Crear(Bombero bombero);

        Task<ResultadoOperacion> Actualizar(Bombero bombero);

        Task<ResultadoOperacion> Desactivar(int id);

        Task<bool> ExisteCedula(string cedula);

        Task<List<Bombero>> ObtenerBomberosDisponibles(
     string? cedula,
     string? nombre,
     string? apellido,
     int? idRango);
    }
}