using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DentAssist.Models.Entities
{
    public class Tratamiento
    {
        [Key]
        public Guid IdTratamiento { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(255)]
        public string Descripcion { get; set; }

        [Required]
        [Precision(10, 2)]
        public decimal PrecioEstimado { get; set; }

        public ICollection<PasoTratamiento> Pasos { get; set; }
        public ICollection<HistorialTratamiento> Historiales { get; set; }
    }
}
