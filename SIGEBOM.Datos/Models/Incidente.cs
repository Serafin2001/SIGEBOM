using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGEBOM.Datos.Models
{
    [Table("Incidentes")]
    public class Incidente
    {
        [Key]
        public int IdIncidente { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(150)]
        public string Direccion { get; set; } = string.Empty;

        [Required]
        public DateTime FechaHoraIncidente { get; set; }

        public DateTime? HoraSalida { get; set; }

        public DateTime? HoraLlegada { get; set; }

        public DateTime? HoraFinalizacion { get; set; }

        [StringLength(500)]
        public string? Descripcion { get; set; }

        // Llave foránea de la llamada
        [Required]
        public int IdLlamada { get; set; }

        // Llave foránea del tipo de incidente
        [Required]
        public int IdTipoIncidente { get; set; }

        // Relaciones
        [ForeignKey(nameof(IdLlamada))]
        public LlamadaEmergencia LlamadaEmergencia { get; set; } = null!;

        [ForeignKey(nameof(IdTipoIncidente))]
        public TipoIncidente TipoIncidente { get; set; } = null!;

        // Relación con Bomberos
        public ICollection<BomberoIncidente> BomberosIncidentes { get; set; } = new List<BomberoIncidente>();

        // Relación con Vehículos
        public ICollection<VehiculoIncidente> VehiculosIncidentes { get; set; } = new List<VehiculoIncidente>();
    }
}