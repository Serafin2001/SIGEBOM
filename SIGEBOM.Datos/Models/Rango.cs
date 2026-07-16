using System.ComponentModel.DataAnnotations;

namespace SIGEBOM.Datos.Models
{
    public class Rango
    {
        [Key]
        public int IdRango { get; set; }

        [Required(ErrorMessage = "El nombre del rango es obligatorio.")]
        [StringLength(50)]
        public string NombreRango { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Descripcion { get; set; }

        [Required]
        public int OrdenJerarquico { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = "Activo";

        //==========================
        // Relaciones
        //==========================

        public virtual ICollection<Bombero> Bomberos { get; set; }
            = new List<Bombero>();
    }
}