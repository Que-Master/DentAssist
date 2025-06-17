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
    public class PasoTratamientoController : Controller
    {
        private readonly AppDbContext _context;

        public PasoTratamientoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PasoTratamiento
        // Muestra todos los pasos de tratamiento registrados, incluyendo los datos del tratamiento y plan asociados
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.PasoTratamientos.Include(p => p.Plan).Include(p => p.Tratamiento);
            return View(await appDbContext.ToListAsync());
        }

        // GET: PasoTratamiento/Details/5
        // Muestra los detalles de un paso de tratamiento especifico
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pasoTratamiento = await _context.PasoTratamientos
                .Include(p => p.Plan)
                .Include(p => p.Tratamiento)
                .FirstOrDefaultAsync(m => m.IdPaso == id);
            if (pasoTratamiento == null)
            {
                return NotFound();
            }

            return View(pasoTratamiento);
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarEstado(Guid idPaso, string nuevoEstado)
        {
            var paso = await _context.PasoTratamientos.FindAsync(idPaso);
            if (paso == null)
                return NotFound();

            paso.Estado = nuevoEstado;
            await _context.SaveChangesAsync();

            return RedirectToAction("MisPlanes", "PlanTratamiento");
        }

        public IActionResult AgregarPasos(Guid idPlan)
        {
            ViewData["IdTratamiento"] = new SelectList(_context.Tratamientos, "IdTratamiento", "Nombre");
            ViewData["IdPlan"] = idPlan;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarPasos(PasoTratamiento paso)
        {
            if (ModelState.IsValid)
            {
                paso.IdPaso = Guid.NewGuid();
                _context.PasoTratamientos.Add(paso);
                await _context.SaveChangesAsync();
                // Puedes redirigir a la misma vista para seguir agregando más pasos
                return RedirectToAction("AgregarPasos", new { idPlan = paso.IdPlan });
            }

            ViewData["IdTratamiento"] = new SelectList(_context.Tratamientos, "IdTratamiento", "Nombre", paso.IdTratamiento);
            ViewData["IdPlan"] = paso.IdPlan;
            return View(paso);
        }


        // GET: PasoTratamiento/Create
        // Retorna la vista para crear un nuevo paso de tratamiento
        public IActionResult Create()
        {
            ViewData["IdPlan"] = new SelectList(_context.PlanTratamientos, "IdPlan", "IdPlan");
            ViewData["IdTratamiento"] = new SelectList(_context.Tratamientos, "IdTratamiento", "Nombre");
            return View();
        }

        // POST: PasoTratamiento/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // Recibe los datos del formulario y guarda un nuevo paso de tratamiento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPaso,IdPlan,IdTratamiento,FechaEstimada,Estado,Observacion")] PasoTratamiento pasoTratamiento)
        {
            if (ModelState.IsValid)
            {
                pasoTratamiento.IdPaso = Guid.NewGuid();
                _context.Add(pasoTratamiento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdPlan"] = new SelectList(_context.PlanTratamientos, "IdPlan", "IdPlan", pasoTratamiento.IdPlan);
            ViewData["IdTratamiento"] = new SelectList(_context.Tratamientos, "IdTratamiento", "Nombre", pasoTratamiento.IdTratamiento);
            return View(pasoTratamiento);
        }

        // GET: PasoTratamiento/Edit/5
        // Muestra la vista de edicion de un paso de tratamiento
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pasoTratamiento = await _context.PasoTratamientos.FindAsync(id);
            if (pasoTratamiento == null)
            {
                return NotFound();
            }
            ViewData["IdPlan"] = new SelectList(_context.PlanTratamientos, "IdPlan", "IdPlan", pasoTratamiento.IdPlan);
            ViewData["IdTratamiento"] = new SelectList(_context.Tratamientos, "IdTratamiento", "Nombre", pasoTratamiento.IdTratamiento);
            return View(pasoTratamiento);
        }

        // POST: PasoTratamiento/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // Procesa la edicion del paso de tratamiento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdPaso,IdPlan,IdTratamiento,FechaEstimada,Estado,Observacion")] PasoTratamiento pasoTratamiento)
        {
            if (id != pasoTratamiento.IdPaso)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pasoTratamiento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PasoTratamientoExists(pasoTratamiento.IdPaso))
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
            ViewData["IdPlan"] = new SelectList(_context.PlanTratamientos, "IdPlan", "IdPlan", pasoTratamiento.IdPlan);
            ViewData["IdTratamiento"] = new SelectList(_context.Tratamientos, "IdTratamiento", "Nombre", pasoTratamiento.IdTratamiento);
            return View(pasoTratamiento);
        }

        // GET: PasoTratamiento/Delete/5
        // Muestra una vista para confirmar la eliminacion de un paso de tratamiento
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pasoTratamiento = await _context.PasoTratamientos
                .Include(p => p.Plan)
                .Include(p => p.Tratamiento)
                .FirstOrDefaultAsync(m => m.IdPaso == id);
            if (pasoTratamiento == null)
            {
                return NotFound();
            }

            return View(pasoTratamiento);
        }

        // POST: PasoTratamiento/Delete/5
        // Elimina definitivamente el paso tratamiento (tras la confirmacion)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var pasoTratamiento = await _context.PasoTratamientos.FindAsync(id);
            if (pasoTratamiento != null)
            {
                _context.PasoTratamientos.Remove(pasoTratamiento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Verifica si un paso de tratamiento con ese ID existe
        private bool PasoTratamientoExists(Guid id)
        {
            return _context.PasoTratamientos.Any(e => e.IdPaso == id);
        }
    }
}
