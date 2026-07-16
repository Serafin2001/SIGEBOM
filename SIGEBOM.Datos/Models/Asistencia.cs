using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGEBOM.Datos.Models
{
    public class Asistencia
    {
        [Key]
        public int IdAsistencia { get; set; }

        [Required]
        public DateTime FechaHoraRegistro { get; set; }

        public TimeSpan? HoraSalida { get; set; }

        [Required]
        [StringLength(20)]
        public string EstadoAsistencia { get; set; } = "Presente";

        [StringLength(255)]
        public string? Observacion { get; set; }

    
        // Llaves Foráneas
   

        [Required]
        [ForeignKey(nameof(DetalleProgramacionTurno))]
        public int IdDetalleProgramacionTurno { get; set; }

        public virtual DetalleProgramacionTurno DetalleProgramacionTurno { get; set; } = null!;

        // Usuario que registró la asistencia
        [Required]
        [ForeignKey(nameof(Usuario))]
        public int IdUsuario { get; set; }

        public virtual Usuario Usuario { get; set; } = null!;
    }
}