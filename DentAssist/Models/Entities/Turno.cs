using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace DentAssist.Models.Entities
{
    public class Turno
    {
        [Key]
        public Guid IdTurno { get; set; } = Guid.NewGuid();

        [Required]
        public DateTime FechaHora { get; set; }

        [Required]
        public int DuracionMinutos { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; }  // Pendiente, Confirmado, Realizado, Cancelado

        public Guid IdPaciente { get; set; }
        public Guid IdOdontologo { get; set; }

        [ForeignKey("IdPaciente")]
        public Paciente Paciente { get; set; }

        [ForeignKey("IdOdontologo")]
        public Odontologo Odontologo { get; set; }
    }
}
