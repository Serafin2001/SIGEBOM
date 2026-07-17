using SIGEBOM.Datos.Models;

namespace SIGEBOM.Negocio.ViewModels
{
    public class ProgramacionTurnoDetailsViewModel
    {
        public ProgramacionTurno Programacion { get; set; } = null!;

        public List<DetalleProgramacionTurno> Bomberos { get; set; } = new();
    }
}