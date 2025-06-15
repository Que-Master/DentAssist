using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace DentAssist.Models.Entities
{
    public class PlanTratamiento
    {
        [Key]
        public Guid IdPlan { get; set; } = Guid.NewGuid();

        public Guid IdPaciente { get; set; }
        public Guid IdOdontologo { get; set; }

        [ForeignKey("IdPaciente")]
        public Paciente Paciente { get; set; }

        [ForeignKey("IdOdontologo")]
        public Odontologo Odontologo { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [StringLength(255)]
        public string Observaciones { get; set; }

        public ICollection<PasoTratamiento> Pasos { get; set; }
    }
}
