using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DentAssist.Models.Entities
{
    public class HistorialTratamiento
    {
        [Key]
        public Guid IdHistorial { get; set; } = Guid.NewGuid();

        public Guid IdPaciente { get; set; }
        public Guid IdTratamiento { get; set; }
        public Guid IdOdontologo { get; set; }
        
        [ForeignKey("IdPaciente")]
        public Paciente Paciente { get; set; }
        
        [ForeignKey("IdTratamiento")]
        [Required]
        public Tratamiento Tratamiento { get; set; }

        [Required]
        [ForeignKey("IdOdontologo")]
        public Odontologo Odontologo { get; set; }

        [Required]
        public DateTime FechaRealizada { get; set; }

        [Required]
        [StringLength(255)]
        public string Observacion { get; set; }

        public Guid? IdPasoTratamiento { get; set; }

        [ForeignKey("IdPasoTratamiento")]
        public PasoTratamiento PasoTratamiento { get; set; }
    }
}
