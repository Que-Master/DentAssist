
using Microsoft.EntityFrameworkCore;
using DentAssist.Models.Entities;

namespace DentAssist.Models.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Odontologo> Odontologos { get; set; }
        public DbSet<Turno> Turnos { get; set; }
        public DbSet<Tratamiento> Tratamientos { get; set; }
        public DbSet<PlanTratamiento> PlanTratamientos { get; set; }
        public DbSet<PasoTratamiento> PasoTratamientos { get; set; }
        public DbSet<HistorialTratamiento> HistorialTratamientos { get; set; }
        public DbSet<Recepcionista> Recepcionistas { get; set; }
    }
}
