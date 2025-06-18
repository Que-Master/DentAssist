using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DentAssist.Models.Data;
using DentAssist.Models.Entities;

namespace DentAssist.Controllers
{
    public class PlanTratamientoController : Controller
    {
        private readonly AppDbContext _context;

        public PlanTratamientoController(AppDbContext context)
        {
            _context = context;
        }

        
        // Muestra la lista de planes de tratamientos registrados
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.PlanTratamientos
                .Include(p => p.Odontologo)
                .Include(p => p.Paciente);
            return View(await appDbContext.ToListAsync());
        }

        
        // Muestra los detalles de un plan de tratamiento específico
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var planTratamiento = await _context.PlanTratamientos
                .Include(p => p.Odontologo)
                .Include(p => p.Paciente)
                .Include(p => p.Pasos)
                    .ThenInclude(p => p.Tratamiento)
                .FirstOrDefaultAsync(m => m.IdPlan == id);

            if (planTratamiento == null) return NotFound();

            return View(planTratamiento);
        }

        
        // Muestra el formulario para crear un nuevo plan de tratamiento
        public IActionResult Create()
        {
            var odontologoId = HttpContext.Session.GetString("OdontologoId");
            if (odontologoId == null)
                return RedirectToAction("Login", "Odontologo");

            // Obtener pacientes que han tenido turnos con este odontólogo
            var pacientes = _context.Turnos
                .Where(t => t.IdOdontologo.ToString() == odontologoId)
                .Select(t => t.Paciente)
                .Distinct()
                .ToList();

            ViewData["IdPaciente"] = new SelectList(pacientes, "IdPaciente", "Nombre");

            return View();
        }

        // Procesa el formulario para crear un nuevo plan de tratamiento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlanTratamiento plan)
        {
            var odontologoId = HttpContext.Session.GetString("OdontologoId");
            if (odontologoId == null)
                return RedirectToAction("Login", "Odontologo");

            if (ModelState.IsValid)
            {
                plan.IdPlan = Guid.NewGuid();
                plan.IdOdontologo = Guid.Parse(odontologoId);
                plan.FechaCreacion = DateTime.Now;

                _context.Add(plan);
                await _context.SaveChangesAsync();

                // Redirige a agregar pasos para el plan recién creado
                return RedirectToAction("AgregarPasos", new { idPlan = plan.IdPlan });
            }

            // Si hay error, recargar pacientes
            var pacientes = _context.Turnos
                .Where(t => t.IdOdontologo.ToString() == odontologoId)
                .Select(t => t.Paciente)
                .Distinct()
                .ToList();

            ViewData["IdPaciente"] = new SelectList(pacientes, "IdPaciente", "Nombre", plan.IdPaciente);
            return View(plan);
        }

        
        // Muestra el formulario para agregar pasos a un plan de tratamiento
        [HttpGet]
        public IActionResult AgregarPasos(Guid idPlan)
        {
            ViewData["IdTratamiento"] = new SelectList(_context.Tratamientos, "IdTratamiento", "Nombre");
            ViewData["IdPlan"] = idPlan;

            // Cargar pasos existentes (opcional)
            var pasos = _context.PasoTratamientos
                .Where(p => p.IdPlan == idPlan)
                .Include(p => p.Tratamiento)
                .ToList();

            ViewData["PasosExistentes"] = pasos;

            return View();
        }

        
        // Procesa la adición de un nuevo paso al plan de tratamiento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarPasos(PasoTratamiento paso)
        {
            if (ModelState.IsValid)
            {
                paso.IdPaso = Guid.NewGuid();
                _context.Add(paso);
                await _context.SaveChangesAsync();

                // Redirige para agregar más pasos al mismo plan
                return RedirectToAction("AgregarPasos", new { idPlan = paso.IdPlan });
            }

            ViewData["IdTratamiento"] = new SelectList(_context.Tratamientos, "IdTratamiento", "Nombre", paso.IdTratamiento);
            ViewData["IdPlan"] = paso.IdPlan;
            return View(paso);
        }

        
        // Muestra los planes creados por el odontólogo en sesión
        public async Task<IActionResult> MisPlanes()
        {
            var odontologoId = HttpContext.Session.GetString("OdontologoId");
            if (odontologoId == null)
                return RedirectToAction("Login", "Odontologo");

            var planes = await _context.PlanTratamientos
                .Where(p => p.IdOdontologo.ToString() == odontologoId)
                .Include(p => p.Paciente)
                .Include(p => p.Pasos)
                    .ThenInclude(p => p.Tratamiento)
                .ToListAsync();

            return View(planes);
        }

        
        // Muestra los detalles extendidos de un plan
        public async Task<IActionResult> Detalles(Guid id)
        {
            var plan = await _context.PlanTratamientos
                .Include(p => p.Paciente)
                .Include(p => p.Odontologo)
                .Include(p => p.Pasos)
                    .ThenInclude(p => p.Tratamiento)
                .FirstOrDefaultAsync(p => p.IdPlan == id);

            if (plan == null) return NotFound();

            return View(plan);
        }

        
        // Muestra el formulario para editar un plan existente
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var planTratamiento = await _context.PlanTratamientos.FindAsync(id);
            if (planTratamiento == null) return NotFound();

            ViewData["IdOdontologo"] = new SelectList(_context.Odontologos, "IdOdontologo", "Matricula", planTratamiento.IdOdontologo);
            ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "IdPaciente", "Nombre", planTratamiento.IdPaciente);

            return View(planTratamiento);
        }
        
        // Procesa la edición de un plan de tratamiento existente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdPlan,IdPaciente,IdOdontologo,FechaCreacion,Observaciones")] PlanTratamiento planTratamiento)
        {
            if (id != planTratamiento.IdPlan)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(planTratamiento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanTratamientoExists(planTratamiento.IdPlan))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdOdontologo"] = new SelectList(_context.Odontologos, "IdOdontologo", "Matricula", planTratamiento.IdOdontologo);
            ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "IdPaciente", "Nombre", planTratamiento.IdPaciente);

            return View(planTratamiento);
        }

        
        // Muestra la vista de confirmación para eliminar un plan
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var planTratamiento = await _context.PlanTratamientos
                .Include(p => p.Odontologo)
                .Include(p => p.Paciente)
                .FirstOrDefaultAsync(m => m.IdPlan == id);

            if (planTratamiento == null) return NotFound();

            return View(planTratamiento);
        }

        
        // Elimina un plan de tratamiento de la base de datos
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var planTratamiento = await _context.PlanTratamientos.FindAsync(id);
            if (planTratamiento != null)
            {
                _context.PlanTratamientos.Remove(planTratamiento);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Verifica si un plan de tratamiento con el ID proporcionado existe
        private bool PlanTratamientoExists(Guid id)
        {
            return _context.PlanTratamientos.Any(e => e.IdPlan == id);
        }
    }
}
