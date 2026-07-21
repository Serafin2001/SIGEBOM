
using System.ComponentModel.DataAnnotations;

namespace SIGEBOM.Negocio.ViewModels
{
    public class GenerarSemanaViewModel
    {
        [Required(ErrorMessage = "Debe seleccionar la fecha de inicio.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de inicio (Lunes)")]
        public DateTime FechaInicio { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Seleccione el encargado del Turno A.")]
        public int IdEncargadoTurnoA { get; set; }

        [Required(ErrorMessage = "Seleccione el encargado del Turno B.")]
        public int IdEncargadoTurnoB { get; set; }

        [Required(ErrorMessage = "Seleccione el encargado del Turno C.")]
        public int IdEncargadoTurnoC { get; set; }

      
    }
}