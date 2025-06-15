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
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.PlanTratamientos.Include(p => p.Odontologo).Include(p => p.Paciente);
            return View(await appDbContext.ToListAsync());
        }

        // GET: PlanTratamiento/Details/5
        public async Task<IActionResult> Details(Guid? id)
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

        // GET: PlanTratamiento/Create
        public IActionResult Create()
        {
            ViewData["IdOdontologo"] = new SelectList(_context.Odontologos, "IdOdontologo", "Matricula");
            ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "IdPaciente", "Nombre");
            return View();
        }

        // POST: PlanTratamiento/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPlan,IdPaciente,IdOdontologo,FechaCreacion,Observaciones")] PlanTratamiento planTratamiento)
        {
            if (ModelState.IsValid)
            {
                planTratamiento.IdPlan = Guid.NewGuid();
                _context.Add(planTratamiento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdOdontologo"] = new SelectList(_context.Odontologos, "IdOdontologo", "Matricula", planTratamiento.IdOdontologo);
            ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "IdPaciente", "Nombre", planTratamiento.IdPaciente);
            return View(planTratamiento);
        }

        // GET: PlanTratamiento/Edit/5
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

        private bool PlanTratamientoExists(Guid id)
        {
            return _context.PlanTratamientos.Any(e => e.IdPlan == id);
        }
    }
}
