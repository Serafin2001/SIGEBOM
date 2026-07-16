using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGEBOM.Datos.Models
{
    [Table("Vehiculos")]
    public class Vehiculo
    {
        [Key]
        public int IdVehiculo { get; set; }

        [Required(ErrorMessage = "El tipo de vehículo es obligatorio.")]
        [StringLength(40)]
        public string TipoVehiculo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La placa es obligatoria.")]
        [StringLength(15)]
        public string Placa { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Marca { get; set; }

        [StringLength(50)]
        public string? Modelo { get; set; }

        [Range(1900, 2100, ErrorMessage = "El año no es válido.")]
        public int? Año { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [StringLength(20)]
        public string Estado { get; set; } = "Activo";

        // Relaciones
        public virtual ICollection<Mantenimiento> Mantenimientos { get; set; }
     = new List<Mantenimiento>();

        public virtual ICollection<VehiculoIncidente> VehiculosIncidentes { get; set; }
            = new List<VehiculoIncidente>();

        public virtual ICollection<InspeccionVehiculo> InspeccionesVehiculo { get; set; }
            = new List<InspeccionVehiculo>();
    }
}