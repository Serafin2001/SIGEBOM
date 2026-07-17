using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGEBOM.Datos.Models
{
    public class DetalleProgramacionTurno
    {
        [Key]
        public int IdDetalleProgramacionTurno { get; set; }

      

        [Required]
        [ForeignKey(nameof(ProgramacionTurno))]
        public int IdProgramacionTurno { get; set; }

        public virtual ProgramacionTurno ProgramacionTurno { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Bombero))]
        public int IdBombero { get; set; }

        public virtual Bombero Bombero { get; set; } = null!;

        //==========================
        // Información de la asignación
        //==========================

        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = "Asignado";

        [StringLength(255)]
        public string? Observacion { get; set; }
    }
}