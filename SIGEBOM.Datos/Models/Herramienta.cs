
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SIGEBOM.Datos.Models
{

    public class Herramienta
    {
        [Key]
        public int IdHerramienta { get; set; }

        [Required(ErrorMessage = "El nombre de la herramienta es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Descripcion { get; set; }

        [Required]
        public int Stock { get; set; } = 0;
        [Required]
        public int StockMinimo { get; set; } = 5;
        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = "Disponible";

        [StringLength(50)]
        public string? Marca { get; set; }

        [StringLength(50)]
        public string? Modelo { get; set; }

        public DateTime? FechaAdquisicion { get; set; }

        public int? IdProveedor { get; set; }

        [ForeignKey(nameof(IdProveedor))]
        public virtual Proveedor? Proveedor { get; set; }

        public virtual ICollection<Inventario> Inventarios { get; set; }
            = new List<Inventario>();
    }
}