using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGEBOM.Datos.Models
{
    public class Mantenimiento
    {
        [Key]
        public int IdMantenimiento { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        [StringLength(50)]
        public string TipoMantenimiento { get; set; } = string.Empty;
        // Preventivo o Correctivo

        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Costo { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = "Pendiente";
        // Pendiente, En proceso, Finalizado, Cancelado

        // Llave foránea
        [Required]
        public int IdVehiculo { get; set; }

        // Relación con Vehículo
        [ForeignKey(nameof(IdVehiculo))]
        public Vehiculo Vehiculo { get; set; } = null!;
    }
}
