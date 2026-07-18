using SIGEBOM.Datos.Models;
using SIGEBOM.Negocio.DTOs;
using SIGEBOM.Negocio.ViewModels;

namespace SIGEBOM.Negocio.Interfaces
{
    public interface IBomberoIncidenteService
    {
        Task<List<BomberoIncidente>> ObtenerTodos();

        Task<List<BomberoIncidente>> ObtenerPorIncidente(int idIncidente);

        Task<BomberoIncidente?> ObtenerPorId(int id);

        Task<BomberoIncidenteViewModel?> ObtenerViewModelEditar(int id);

        Task<ResultadoOperacion> Crear(BomberoIncidenteViewModel model);

        Task<ResultadoOperacion> Actualizar(BomberoIncidenteViewModel model);

        Task<ResultadoOperacion> CambiarEstado(int id);

        Task<List<BomberoBusquedaViewModel>> BuscarBomberos(string filtro);

        Task<Incidente?> ObtenerIncidente(int idIncidente);
        Task<BomberoIncidenteIndexViewModel?> ObtenerPantalla(int idIncidente);
        
    }
}