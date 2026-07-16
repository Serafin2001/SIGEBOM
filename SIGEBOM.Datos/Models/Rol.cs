using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGEBOM.Datos.Models
{
    [Table("Roles")]
    public class Rol
    {
        [Key]
        public int IdRol { get; set; }

        [Required(ErrorMessage = "El nombre del rol es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre del rol no puede superar los 50 caracteres.")]
        public string NombreRol { get; set; } = string.Empty;

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [StringLength(10)]
        public string Estado { get; set; } = "Activo";

        // Relación: Un rol puede tener muchos usuarios
        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}