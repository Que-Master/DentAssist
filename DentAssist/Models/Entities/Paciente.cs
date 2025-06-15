using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc; 

namespace DentAssist.Models.Entities
{
    public class Paciente
    {
        [Key]
        public Guid IdPaciente { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(12)]
        public string RUT { get; set; }

        [StringLength(15)]
        public string Telefono { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(255)]
        public string Direccion { get; set; }
    }
}
