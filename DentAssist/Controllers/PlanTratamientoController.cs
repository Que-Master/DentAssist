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

        // GET: PlanTratamiento
        // Muestra la lista de planes de tratamientos registrados
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.PlanTratamientos.Include(p => p.Odontologo).Include(p => p.Paciente);
            return View(await appDbContext.ToListAsync());
        }

        // GET: PlanTratamiento/Details/5
        // Muestra los detalles de un plan de tratamiento especifico
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planTratamiento = await _context.PlanTratamientos
                .Include(p => p.Odontologo)
                .Include(p => p.Paciente)
                .Include(p => p.Pasos)
                    .ThenInclude(p => p.Tratamiento)
                .FirstOrDefaultAsync(m => m.IdPlan == id);

            if (planTratamiento == null)
            {
                return NotFound();
            }

            return View(planTratamiento);
        }


        // GET: PlanTratamiento/Create
        // Muestra el formulario para crear un nuevo plan de tratamiento
        public IActionResult Create()
        {
            var odontologoId = HttpContext.Session.GetString("OdontologoId");

            if (odontologoId == null)
                return RedirectToAction("Login", "Odontologo");

            // Buscar solo pacientes que tengan turnos con este odontólogo
            var pacientes = _context.Turnos
                .Where(t => t.IdOdontologo.ToString() == odontologoId)
                .Select(t => t.Paciente)
                .Distinct()
                .ToList();

            ViewData["IdPaciente"] = new SelectList(pacientes, "IdPaciente", "Nombre");

            return View();
        }


        public async Task<IActionResult> Detalles(Guid id)
        {
            var plan = await _context.PlanTratamientos
                .Include(p => p.Paciente)
                .Include(p => p.Odontologo)
                .Include(p => p.Pasos)
                    .ThenInclude(p => p.Tratamiento)
                .FirstOrDefaultAsync(p => p.IdPlan == id);

            if (plan == null)
                return NotFound();

            return View(plan);
        }

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


        // POST: PlanTratamiento/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //Procesa el formulario para crear un nuevo plan de tratamiento

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarPasos(PasoTratamiento paso)
        {
            if (ModelState.IsValid)
            {
                paso.IdPaso = Guid.NewGuid();
                _context.Add(paso);
                await _context.SaveChangesAsync();

                // Redirige para agregar otro paso al mismo plan
                return RedirectToAction("AgregarPasos", new { idPlan = paso.IdPlan });
            }

            ViewData["IdTratamiento"] = new SelectList(_context.Tratamientos, "IdTratamiento", "Nombre", paso.IdTratamiento);
            ViewData["IdPlan"] = paso.IdPlan;
            return View(paso);
        }
        // GET: PlanTratamiento/AgregarPasos?idPlan=xxxxx
        [HttpGet]
        public IActionResult AgregarPasos(Guid idPlan)
        {
            ViewData["IdTratamiento"] = new SelectList(_context.Tratamientos, "IdTratamiento", "Nombre");
            ViewData["IdPlan"] = idPlan;

            // Cargar los pasos existentes para mostrarlos (opcional)
            var pasos = _context.PasoTratamientos
                .Where(p => p.IdPlan == idPlan)
                .Include(p => p.Tratamiento)
                .ToList();

            ViewData["PasosExistentes"] = pasos;

            return View();
        }


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

                return RedirectToAction("AgregarPasos", new { idPlan = plan.IdPlan });
            }

            // Recargar los pacientes si falla
            var pacientes = _context.Turnos
                .Where(t => t.IdOdontologo.ToString() == odontologoId)
                .Select(t => t.Paciente)
                .Distinct()
                .ToList();

            ViewData["IdPaciente"] = new SelectList(pacientes, "IdPaciente", "Nombre", plan.IdPaciente);
            return View(plan);
        }



        // GET: PlanTratamiento/Edit/5
        // Muestra el formulario para editar un plan de tratamiento existente
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planTratamiento = await _context.PlanTratamientos.FindAsync(id);
            if (planTratamiento == null)
            {
                return NotFound();
            }
            ViewData["IdOdontologo"] = new SelectList(_context.Odontologos, "IdOdontologo", "Matricula", planTratamiento.IdOdontologo);
            ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "IdPaciente", "Nombre", planTratamiento.IdPaciente);
            return View(planTratamiento);
        }

        // POST: PlanTratamiento/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // Procesa la edicion del plan de tratamiento 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdPlan,IdPaciente,IdOdontologo,FechaCreacion,Observaciones")] PlanTratamiento planTratamiento)
        {
            if (id != planTratamiento.IdPlan)
            {
                return NotFound();
            }

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
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdOdontologo"] = new SelectList(_context.Odontologos, "IdOdontologo", "Matricula", planTratamiento.IdOdontologo);
            ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "IdPaciente", "Nombre", planTratamiento.IdPaciente);
            return View(planTratamiento);
        }

        // GET: PlanTratamiento/Delete/5
        // Muestra una vista para confirmar la eliminacion de un plan de tratamiento 
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planTratamiento = await _context.PlanTratamientos
                .Include(p => p.Odontologo)
                .Include(p => p.Paciente)
                .FirstOrDefaultAsync(m => m.IdPlan == id);
            if (planTratamiento == null)
            {
                return NotFound();
            }

            return View(planTratamiento);
        }

        // POST: PlanTratamiento/Delete/5
        // Elimina definitivamente el plan de tratamiento (tras la confirmacion)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var planTratamiento = await _context.PlanTratamientos.FindAsync(id);
            if (planTratamiento != null)
            {
                _context.PlanTratamientos.Remove(planTratamiento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Verifica si un plan de tratamiento con ese ID existe
        private bool PlanTratamientoExists(Guid id)
        {
            return _context.PlanTratamientos.Any(e => e.IdPlan == id);
        }
    }
}
