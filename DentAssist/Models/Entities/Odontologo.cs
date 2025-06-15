using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace DentAssist.Models.Entities
{
    public class Odontologo
    {
        [Key]
        public Guid IdOdontologo { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50)]
        public string Matricula { get; set; }
        [Required]
        [StringLength(100)]
        public string Especialidad { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        public string Contrasenia { get; set; }

        public ICollection<Turno> Turnos { get; set; }
        public ICollection<PlanTratamiento> Planes { get; set; }
        public ICollection<HistorialTratamiento> Historiales { get; set; }
    }
}
