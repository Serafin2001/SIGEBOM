using System.ComponentModel.DataAnnotations;

namespace SIGEBOM.Datos.Models
{
    public class Proveedor
    {
        [Key]
        public int IdProveedor { get; set; }

        [Required(ErrorMessage = "El nombre del proveedor es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(15)]
        public string? Telefono { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = "Activo";

        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo válido.")]
        public string? Correo { get; set; }

        [StringLength(150)]
        public string? Direccion { get; set; }

        public DateTime? Fecha { get; set; }

        // Relación: Un proveedor puede suministrar muchas herramientas
        public virtual ICollection<Herramienta> Herramientas { get; set; }
            = new List<Herramienta>();
    }
}