using System.ComponentModel.DataAnnotations;

namespace SIGEBOM.Negocio.ViewModels
{
    public class BomberoIncidenteViewModel
    {
        public int IdBomberoIncidente { get; set; }

        [Required]
        public int IdIncidente { get; set; }

        [Display(Name = "No. Incidente")]
        public int NumeroIncidente { get; set; }

        [Required]
        public int IdBombero { get; set; }

        [Display(Name = "Bombero")]
        public string NombreBombero { get; set; } = string.Empty;

        [Required(ErrorMessage = "La función es obligatoria.")]
        [StringLength(100)]
        [Display(Name = "Función")]
        public string Funcion { get; set; } = string.Empty;

        [StringLength(300)]
        [Display(Name = "Observación")]
        public string? Observacion { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Activo";
    }
}