using SIGEBOM.Datos.Models;
using SIGEBOM.Negocio.DTOs;
using SIGEBOM.Negocio.ViewModels;

namespace SIGEBOM.Negocio.Interfaces
{
    public interface ILlamadaEmergenciaService
    {
     

        Task<List<LlamadaEmergencia>> ObtenerTodas();

        Task<LlamadaEmergencia?> ObtenerPorId(int id);

        Task<LlamadaEmergenciaViewModel?> ObtenerViewModelEditar(int id);


        Task<ResultadoOperacion> Crear(
            LlamadaEmergenciaViewModel model,
            int idUsuario);

        Task<ResultadoOperacion> Actualizar(
            LlamadaEmergenciaViewModel model);

        Task<ResultadoOperacion> CambiarEstado(
            int id,
            string estado);

    }
}