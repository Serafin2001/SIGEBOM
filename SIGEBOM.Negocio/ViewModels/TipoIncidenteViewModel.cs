using System.ComponentModel.DataAnnotations;

namespace SIGEBOM.Negocio.ViewModels
{
    public class TipoIncidenteViewModel
    {
        public int IdTipoIncidente { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(255)]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Activo";

        [Required(ErrorMessage = "El código es obligatorio.")]
        [StringLength(10)]
        [Display(Name = "Código")]
        public string Codigo { get; set; } = string.Empty;
    } 
}
