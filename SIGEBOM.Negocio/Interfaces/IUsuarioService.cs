using SIGEBOM.Datos.Models;
using SIGEBOM.Negocio.DTOs;

namespace SIGEBOM.Negocio.Interfaces
{
    public interface IUsuarioService
    {
        // Listado
        Task<List<Usuario>> ObtenerTodos(string? buscar);

        // Obtener un usuario por Id
        Task<Usuario?> ObtenerPorId(int id);

        // Registrar usuario
        Task<ResultadoOperacion> Crear(Usuario usuario);

        // Editar usuario
        Task<ResultadoOperacion> Actualizar(Usuario usuario);

        // Desactivar usuario
        Task<ResultadoOperacion> Desactivar(int id);

        // Validaciones
        Task<bool> ExisteUsuario(string nombreUsuario);

        Task<bool> ExisteBombero(int idBombero);

        Task<ResultadoOperacion> CambiarPassword(int idUsuario, string nuevaPassword);
        Task<Usuario?> Login(string nombreUsuario, string contraseña);
    }
}