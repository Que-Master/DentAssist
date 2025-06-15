using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace DentAssist.Models.Entities
{
    public class PasoTratamiento
    {
        [Key]
        public Guid IdPaso { get; set; } = Guid.NewGuid();

        public Guid IdPlan { get; set; }
        public Guid IdTratamiento { get; set; }

        [ForeignKey("IdPlan")]
        public PlanTratamiento Plan { get; set; }

        [ForeignKey("IdTratamiento")]
        public Tratamiento Tratamiento { get; set; }

        public DateTime? FechaEstimada { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; } 

        [StringLength(255)]
        public string Observacion { get; set; }
    }
}
