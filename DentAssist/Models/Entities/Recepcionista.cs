using System.ComponentModel.DataAnnotations;
using DentAssist.Models.Entities;

namespace DentAssist.Models.Entities
{
    public class Recepcionista
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Email { get; set; }

        public string Contrasenia { get; set; }

        public ICollection<Turno> TurnosAgendados { get; set; }
    }
}