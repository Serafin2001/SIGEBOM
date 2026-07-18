using SIGEBOM.Datos.Models;
using SIGEBOM.Negocio.DTOs;
using SIGEBOM.Negocio.ViewModels;

namespace SIGEBOM.Negocio.Interfaces
{
    public interface ITipoIncidenteService
    {
        //=========================================
        // CONSULTAS
        //=========================================

        Task<List<TipoIncidente>> ObtenerTodos();

        Task<TipoIncidente?> ObtenerPorId(int id);

        Task<TipoIncidenteViewModel?> ObtenerViewModelEditar(int id);

        //=========================================
        // MANTENIMIENTO
        //=========================================

        Task<ResultadoOperacion> Crear(
            TipoIncidenteViewModel model);

        Task<ResultadoOperacion> Actualizar(
            TipoIncidenteViewModel model);

        Task<ResultadoOperacion> CambiarEstado(
            int id,
            string estado);
    }
}