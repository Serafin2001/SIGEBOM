using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGEBOM.Datos.Models
{
    public class DetalleInspeccion
    {
        [Key]
        public int IdDetalleInspeccion { get; set; }

        // Llave foránea
        [Required]
        public int IdInspeccion { get; set; }

        [Required]
        [StringLength(100)]
        public string ElementoRevisado { get; set; } = string.Empty;
        // Ejemplo: Gas Oil, Aceite de motor, Gomas, Sirena

        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = "Bueno";
        // Bueno, Malo, No aplica

        [StringLength(255)]
        public string? Observacion { get; set; }

        // Relación con InspeccionVehiculo
        [ForeignKey(nameof(IdInspeccion))]
        public InspeccionVehiculo InspeccionVehiculo { get; set; } = null!;
    }
}
