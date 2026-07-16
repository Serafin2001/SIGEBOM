using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGEBOM.Datos.Models
{
    [Table("Bomberos")]
    public class Bombero
    {
        [Key]
        public int IdBombero { get; set; }

        [Required(ErrorMessage = "La cédula es obligatoria.")]
        [StringLength(13)]
        public string Cedula { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(50)]
        public string Apellido { get; set; } = string.Empty;

        [StringLength(15)]
        public string? Sexo { get; set; }

        [Required]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [StringLength(15)]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(150)]
        public string Direccion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de sangre es obligatorio.")]
        [StringLength(5)]
        public string TipoSangre { get; set; } = string.Empty;

        [Required]
        public DateTime FechaIngreso { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = "Activo";

        [StringLength(255)]
        public string? MotivoEstado { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? Correo { get; set; }

       
        // Llaves Foráneas
    
        public int? IdCargo { get; set; }

        [ForeignKey(nameof(IdCargo))]
        public virtual Cargo? Cargo { get; set; }

        [Required]
        public int IdRango { get; set; }

        [ForeignKey(nameof(IdRango))]
        public virtual Rango? Rango { get; set; }


        // Relaciones


        // Relación 1:1 con Usuario
        public virtual Usuario? Usuario { get; set; }

        // Incidentes
        public virtual ICollection<BomberoIncidente> BomberosIncidentes { get; set; }
            = new List<BomberoIncidente>();

        // Capacitaciones
        public virtual ICollection<BomberoCapacitacion> BomberosCapacitaciones { get; set; }
            = new List<BomberoCapacitacion>();

        // Inspecciones de vehículos
        public virtual ICollection<InspeccionVehiculo> InspeccionesVehiculos { get; set; }
            = new List<InspeccionVehiculo>();

        // Programaciones donde es el encargado
        public virtual ICollection<ProgramacionTurno> ProgramacionesComoEncargado { get; set; }
            = new List<ProgramacionTurno>();

        // Programaciones donde participa
        public virtual ICollection<DetalleProgramacionTurno> ProgramacionesAsignadas { get; set; }
            = new List<DetalleProgramacionTurno>();

        //==========================
        // Propiedades calculadas
        //==========================

        [NotMapped]
        public string NombreCompleto => $"{Nombre} {Apellido}";
    }
}