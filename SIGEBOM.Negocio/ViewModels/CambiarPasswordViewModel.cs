using System.ComponentModel.DataAnnotations;

namespace SIGEBOM.Negocio.ViewModels
{
    public class CambiarPasswordViewModel
    {
        public int IdUsuario { get; set; }

        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La nueva contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8)]
        public string NuevaContraseña { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe confirmar la contraseña.")]
        [DataType(DataType.Password)]
        [Compare(nameof(NuevaContraseña),
            ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmarContraseña { get; set; } = string.Empty;
    }
}