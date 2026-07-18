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
    
        public TimeSpan HoraSalida { get; set; }

        [Required]
        public TimeSpan HoraLlegada { get; set; }

        [Required]
        public TimeSpan HoraFinalizacion { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = "Pendiente";

        [StringLength(500)]
        public string? ObservacionesFinales { get; set; }

        [Display(Name = "Fecha y Hora")]
        public DateTime FechaHoraIncidente { get; set; } = DateTime.Now;

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