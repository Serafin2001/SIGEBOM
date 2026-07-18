using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGEBOM.Datos.Models
{
    [Table("LlamadasEmergencia")]
    public class LlamadaEmergencia
    {
        [Key]
        public int IdLlamada { get; set; }

        [Required(ErrorMessage = "El nombre del reportante es obligatorio.")]
        [StringLength(100)]
        public string NombreReportante { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [StringLength(15)]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(150)]
        public string Direccion { get; set; } = string.Empty;

        [Required]
        public DateTime FechaHora { get; set; } = DateTime.Now;

        [StringLength(255)]
        public string? Observacion { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = "Pendiente";

        // Llave foránea del usuario que registró la llamada
        [Required]
        public int IdUsuario { get; set; }

        // Llave foránea del tipo de incidente
        [Required]
        public int IdTipoIncidente { get; set; }

        // Relaciones
        [ForeignKey(nameof(IdUsuario))]
        public Usuario Usuario { get; set; } = null!;

        [ForeignKey(nameof(IdTipoIncidente))]
        public TipoIncidente TipoIncidente { get; set; } = null!;
    }
}