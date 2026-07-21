using System.ComponentModel.DataAnnotations;

namespace SIGEBOM.Negocio.ViewModels
{
    public class IncidenteViewModel
    {
        public int IdIncidente { get; set; }

        [Required(ErrorMessage = "La llamada es obligatoria.")]
        [Display(Name = "No. Llamada")]
        public int IdLlamada { get; set; }

        [Required(ErrorMessage = "El tipo de incidente es obligatorio.")]
        public int IdTipoIncidente { get; set; }

        // Solo para mostrar el nombre del tipo en la vista
        [Display(Name = "Tipo de Incidente")]
        public string NombreTipoIncidente { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [Display(Name = "Dirección")]
        [StringLength(150)]
        public string Direccion { get; set; } = string.Empty;

        [Display(Name = "Fecha y Hora del Incidente")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime FechaHoraIncidente { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "La hora de salida es obligatoria.")]
        [Display(Name = "Hora de Salida")]
        public TimeSpan? HoraSalida { get; set; }

        [Required(ErrorMessage = "La hora de llegada es obligatoria.")]
        [Display(Name = "Hora de Llegada")]
        public TimeSpan? HoraLlegada { get; set; }

        [Required(ErrorMessage = "La hora de finalización es obligatoria.")]
        [Display(Name = "Hora de Finalización")]
        public TimeSpan? HoraFinalizacion { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [Display(Name = "Descripción del Incidente")]
        [StringLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        
        [Required(ErrorMessage = "La programación del turno es obligatoria.")]
        [Display(Name = "Programación del Turno")]
        public int IdProgramacionTurno { get; set; }
    }
}