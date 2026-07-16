using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGEBOM.Datos.Models
{
    public class BomberoCapacitacion
    {
        [Key]
        public int IdBomberoCapacitacion { get; set; }

        [Required]
        public int IdBombero { get; set; }

        [Required]
        public int IdCapacitacion { get; set; }

        [Required]
        public DateTime FechaRealizacion { get; set; }

        [StringLength(255)]
        public string? Observaciones { get; set; }

        // Relación con Bombero
        [ForeignKey(nameof(IdBombero))]
        public Bombero Bombero { get; set; } = null!;

        // Relación con Capacitación
        [ForeignKey(nameof(IdCapacitacion))]
        public Capacitacion Capacitacion { get; set; } = null!;
    }
}
