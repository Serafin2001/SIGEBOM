using SIGEBOM.Datos.Models;

namespace SIGEBOM.Negocio.ViewModels
{
    public class BomberoIncidenteIndexViewModel
    {
        public Incidente Incidente { get; set; } = null!;

        public List<BomberoIncidente> Bomberos { get; set; } = new();

        public BomberoIncidenteViewModel NuevoBombero { get; set; } = new();
    }
}
