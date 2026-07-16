using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGEBOM.Datos.Models
{
    public class InspeccionVehiculo
    {
        [Key]
        public int IdInspeccion { get; set; }

        [Required]
        public int IdVehiculo { get; set; }

        [Required]
        public int IdBombero { get; set; }

        [Required]
        public DateTime FechaHoraInspeccion { get; set; }

        [StringLength(255)]
        public string? Observaciones { get; set; }

        [Required]
        [StringLength(20)]
        public string EstadoGeneral { get; set; } = "Apto";
        // Apto, No apto, En observación

        // Relación con Vehículo
        [ForeignKey(nameof(IdVehiculo))]
        public Vehiculo Vehiculo { get; set; } = null!;

        // Relación con Bombero
        [ForeignKey(nameof(IdBombero))]
        public Bombero Bombero { get; set; } = null!;

        // Relación con DetalleInspeccion
        public ICollection<DetalleInspeccion> DetallesInspeccion { get; set; } = new List<DetalleInspeccion>();
    }
}