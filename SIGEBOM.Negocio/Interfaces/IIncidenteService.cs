using SIGEBOM.Datos.Models;
using SIGEBOM.Negocio.DTOs;
using SIGEBOM.Negocio.ViewModels;

namespace SIGEBOM.Negocio.Interfaces
{
    public interface IIncidenteService
    {
        //=========================================
        // CONSULTAS
        //=========================================

        Task<List<Incidente>> ObtenerTodos();

        Task<Incidente?> ObtenerPorId(int id);

        Task<IncidenteViewModel?> ObtenerViewModelEditar(int id);

        // Obtiene los datos de la llamada para crear un incidente
        Task<IncidenteViewModel?> ObtenerDesdeLlamada(int idLlamada);

        // Verifica si una llamada ya tiene un incidente registrado
        Task<bool> ExisteIncidentePorLlamada(int idLlamada);

        //=========================================
        // MANTENIMIENTO
        //=========================================

        Task<ResultadoOperacion> Crear(IncidenteViewModel model);

        Task<ResultadoOperacion> Actualizar(IncidenteViewModel model);
    }
}