using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGEBOM.Datos.Models
{
    public class TipoIncidente
    {
        [Key]
        public int IdTipoIncidente { get; set; }

        [Required(ErrorMessage = "El nombre del tipo de incidente es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Descripcion { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = "Activo";

        [Display(Name = "Código")]
        public string Codigo { get; set; } = string.Empty;

        // Relaciones
        public virtual ICollection<LlamadaEmergencia> Llamadas { get; set; } = new List<LlamadaEmergencia>();

        public virtual ICollection<Incidente> Incidentes { get; set; } = new List<Incidente>();
    }
}
