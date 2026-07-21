using SIGEBOM.Datos.Models;
using SIGEBOM.Negocio.DTOs;
using SIGEBOM.Negocio.ViewModels;

namespace SIGEBOM.Negocio.Interfaces
{
    public interface IIncidenteService
    {

        Task<List<Incidente>> ObtenerTodos();

        Task<Incidente?> ObtenerPorId(int id);

        Task<IncidenteViewModel?> ObtenerViewModelEditar(int id);

        Task<IncidenteViewModel?> ObtenerDesdeLlamada(int idLlamada);

        Task<bool> ExisteIncidentePorLlamada(int idLlamada);

        Task<ResultadoOperacion> Crear(IncidenteViewModel model);

        Task<ResultadoOperacion> Actualizar(IncidenteViewModel model);
    }
}
