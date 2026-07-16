using System.ComponentModel.DataAnnotations;

namespace SIGEBOM.Datos.Models
{
    public class Capacitacion
    {
        [Key]
        public int IdCapacitacion { get; set; }

        [Required(ErrorMessage = "El nombre de la capacitación es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        [StringLength(100)]
        public string? Institucion { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = "Activa";

        // Relación: Una capacitación puede ser realizada por muchos bomberos
        public ICollection<BomberoCapacitacion> BomberosCapacitaciones { get; set; } = new List<BomberoCapacitacion>();
    }
}