using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGEBOM.Datos.Models
{
    [Table("BomberosIncidentes")]
    public class BomberoIncidente
    {
        [Key]
        public int IdBomberoIncidente { get; set; }

        [StringLength(100)]
        public string? Funcion { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = "Activo";

        [StringLength(300)]
        public string? Observacion { get; set; }

        [Required]
        public int IdBombero { get; set; }

        [Required]
        public int IdIncidente { get; set; }

        [ForeignKey(nameof(IdBombero))]
        public Bombero Bombero { get; set; } = null!;

        [ForeignKey(nameof(IdIncidente))]
        public Incidente Incidente { get; set; } = null!;
    }
}