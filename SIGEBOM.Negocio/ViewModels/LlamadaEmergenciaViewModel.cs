using System.ComponentModel.DataAnnotations;

namespace SIGEBOM.Negocio.ViewModels
{
    public class LlamadaEmergenciaViewModel
    {
        public int IdLlamada { get; set; }

        [Required(ErrorMessage = "El nombre del reportante es obligatorio.")]
        public string NombreReportante { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        public string Direccion { get; set; } = string.Empty;

        public DateTime FechaHora { get; set; } = DateTime.Now;

        public string? Observacion { get; set; }

        public string Estado { get; set; } = "Pendiente";

        public int IdTipoIncidente { get; set; }
    }
}