using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGEBOM.Datos.Models
{
    public class Usuario
    {
  

        [Key]
        public int IdUsuario { get; set; }



        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(50, MinimumLength = 4)]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [StringLength(255, MinimumLength = 8)]
        public string Contraseña { get; set; } = string.Empty;

        [NotMapped]
        [DataType(DataType.Password)]
        [Compare("Contraseña", ErrorMessage = "Las contraseñas no coinciden.")]
        public string? ConfirmarContraseña { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = "Activo";

        [StringLength(250)]
        public string? MotivoEstado { get; set; }

        [NotMapped]
        public string? NombreBombero { get; set; }

    
        // LLAVES FORÁNEAS


        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un bombero.")]
        public int IdBombero { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un rol.")]
        public int IdRol { get; set; }


        // PROPIEDADES DE NAVEGACIÓN
        [ForeignKey(nameof(IdBombero))]
        public virtual Bombero? Bombero { get; set; }

        [ForeignKey(nameof(IdRol))]
        public virtual Rol? Rol { get; set; }

        // Llamadas de emergencia registradas por el usuario
        public virtual ICollection<LlamadaEmergencia> LlamadasRegistradas { get; set; }
            = new List<LlamadaEmergencia>();

        // Programaciones de turnos creadas por el usuario
        public virtual ICollection<ProgramacionTurno> ProgramacionesCreadas { get; set; }
            = new List<ProgramacionTurno>();
    }
}