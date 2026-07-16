using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGEBOM.Datos.Models
{
    [Table("VehiculosIncidentes")]
    public class VehiculoIncidente
    {
        [Key]
        public int IdVehiculoIncidente { get; set; }

        [StringLength(255)]
        public string? Observaciones { get; set; }

        [Required]
        public int IdVehiculo { get; set; }

        [Required]
        public int IdIncidente { get; set; }

        [ForeignKey(nameof(IdVehiculo))]
        public virtual Vehiculo Vehiculo { get; set; } = null!;

        [ForeignKey(nameof(IdIncidente))]
        public virtual Incidente Incidente { get; set; } = null!;
    }
}