using SIGEBOM.Datos.Models;
using System.ComponentModel.DataAnnotations;

namespace SIGEBOM.Negocio.ViewModels
{
    public class ProgramacionTurnoViewModel
    {


        public int IdProgramacionTurno { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        public DateOnly Fecha { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un turno.")]
        public int IdTurno { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un encargado.")]
        public int IdEncargado { get; set; }

        [StringLength(255)]
        public string? Observacion { get; set; }

        public string Estado { get; set; } = "Programado";


        public string? CedulaEncargado { get; set; }

        public string? NombreEncargado { get; set; }



        public List<int> BomberosSeleccionados { get; set; }
            = new();


        public List<Turno> Turnos { get; set; }
            = new();

        public List<Bombero> Bomberos { get; set; }
            = new();
    }
}