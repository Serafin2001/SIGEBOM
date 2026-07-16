using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SIGEBOM.Datos.Models
{
    public class Cargo
    {
        [Key]
        public int IdCargo { get; set; }

        [Required(ErrorMessage = "El nombre del cargo es obligatorio.")]
        [StringLength(50)]
        public string NombreCargo { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [StringLength(10)]
        public string Estado { get; set; } = string.Empty;

        // Relación: Un cargo puede tener muchos bomberos
        public ICollection<Bombero> Bomberos { get; set; } = new List<Bombero>();
    }
}
