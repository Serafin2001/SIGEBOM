using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGEBOM.Datos.Models
{
    public class Inventario
    {
        [Key]
        public int IdInventario { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una herramienta.")]
        public int IdHerramienta { get; set; }
        [Required]
        [StringLength(30)]
        public string TipoMovimiento { get; set; } = string.Empty;

        [Required]
        public int Cantidad { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [StringLength(255)]
        public string? Observacion { get; set; }


        [ForeignKey(nameof(IdHerramienta))]
     
        public Herramienta Herramienta { get; set; } = null!;
    }
}

