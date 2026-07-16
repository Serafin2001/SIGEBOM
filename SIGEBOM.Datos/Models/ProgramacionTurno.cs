using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGEBOM.Datos.Models
{
    public class ProgramacionTurno
    {
        [Key]
        public int IdProgramacionTurno { get; set; }

        [Required(ErrorMessage = "La fecha de la programación es obligatoria.")]
        public DateOnly Fecha { get; set; }

        [StringLength(255)]
        public string? Observacion { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = "Programado";

        // Fecha en que se creó la programación
        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        //==========================
        // Llaves Foráneas
        //==========================

        [Required]
        [ForeignKey(nameof(Turno))]
        public int IdTurno { get; set; }

        public virtual Turno Turno { get; set; } = null!;

        // Bombero encargado del turno
        [Required]
        [ForeignKey(nameof(Encargado))]
        public int IdEncargado { get; set; }

        public virtual Bombero Encargado { get; set; } = null!;

        // Usuario que registró la programación
        [Required]
        [ForeignKey(nameof(UsuarioCreador))]
        public int IdUsuarioCreador { get; set; }

        public virtual Usuario UsuarioCreador { get; set; } = null!;

        //==========================
        // Relaciones
        //==========================

        public virtual ICollection<DetalleProgramacionTurno> DetallesProgramacion { get; set; }
            = new List<DetalleProgramacionTurno>();
    }
}