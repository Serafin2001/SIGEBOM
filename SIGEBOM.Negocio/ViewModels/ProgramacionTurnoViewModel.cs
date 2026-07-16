using System.ComponentModel.DataAnnotations;

namespace SIGEBOM.Negocio.ViewModels
{
    public class ProgramacionTurnoViewModel
    {
        public int IdProgramacion { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Debe seleccionar un turno.")]
        public int IdTurno { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un encargado.")]
        public int IdEncargado { get; set; }

        public string? NombreEncargado { get; set; }

        public List<int> BomberosSeleccionados { get; set; } = new();

        public string Estado { get; set; } = "Activo";
    }
}