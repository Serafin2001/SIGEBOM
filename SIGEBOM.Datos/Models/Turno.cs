using System.ComponentModel.DataAnnotations;

namespace SIGEBOM.Datos.Models
{
    public class Turno
    {
        [Key]
        public int IdTurno { get; set; }

        [Required(ErrorMessage = "El nombre del turno es obligatorio.")]
        [StringLength(50)]
        public string NombreTurno { get; set; } = string.Empty;

        [Required(ErrorMessage = "La hora de entrada es obligatoria.")]
        public TimeSpan HoraEntrada { get; set; }

        [Required(ErrorMessage = "La hora de salida es obligatoria.")]
        public TimeSpan HoraSalida { get; set; }

        [StringLength(255)]
        public string? Descripcion { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = "Activo";

        // Relación: Un turno puede tener muchas programaciones.
        public virtual ICollection<ProgramacionTurno> ProgramacionesTurno { get; set; }
            = new List<ProgramacionTurno>();
    }
}