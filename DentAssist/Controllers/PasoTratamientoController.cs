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

        
        // Muestra todos los pasos de tratamiento registrados, incluyendo los datos del tratamiento y plan asociados
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.PasoTratamientos
                .Include(p => p.Plan)
                .Include(p => p.Tratamiento);
            return View(await appDbContext.ToListAsync());
        }

        
        // Muestra los detalles de un paso de tratamiento específico
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
                return NotFound();

            var pasoTratamiento = await _context.PasoTratamientos
                .Include(p => p.Plan)
                .Include(p => p.Tratamiento)
                .FirstOrDefaultAsync(m => m.IdPaso == id);

            if (pasoTratamiento == null)
                return NotFound();

            return View(pasoTratamiento);
        }

        // Permite cambiar el estado de un paso de tratamiento (por ejemplo: Pendiente, En Proceso, Completado)
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

        
        // Muestra el formulario para agregar un nuevo paso a un plan de tratamiento específico
        public IActionResult AgregarPasos(Guid idPlan)
        {
            ViewData["IdTratamiento"] = new SelectList(_context.Tratamientos, "IdTratamiento", "Nombre");
            ViewData["IdPlan"] = idPlan;
            return View();
        }

        
        // Guarda un nuevo paso al plan de tratamiento y permite seguir agregando más pasos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarPasos(PasoTratamiento paso)
        {
            if (ModelState.IsValid)
            {
                paso.IdPaso = Guid.NewGuid();
                _context.PasoTratamientos.Add(paso);
                await _context.SaveChangesAsync();
                return RedirectToAction("AgregarPasos", new { idPlan = paso.IdPlan });
            }

            ViewData["IdTratamiento"] = new SelectList(_context.Tratamientos, "IdTratamiento", "Nombre", paso.IdTratamiento);
            ViewData["IdPlan"] = paso.IdPlan;
            return View(paso);
        }

        
        // Muestra el formulario para registrar un nuevo paso de tratamiento
        public IActionResult Create()
        {
            ViewData["IdPlan"] = new SelectList(_context.PlanTratamientos, "IdPlan", "IdPlan");
            ViewData["IdTratamiento"] = new SelectList(_context.Tratamientos, "IdTratamiento", "Nombre");
            return View();
        }

        
        // Guarda un nuevo paso de tratamiento en la base de datos
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

        // Muestra el formulario para editar un paso de tratamiento
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
                return NotFound();

            var pasoTratamiento = await _context.PasoTratamientos.FindAsync(id);
            if (pasoTratamiento == null)
                return NotFound();

            ViewData["IdPlan"] = new SelectList(_context.PlanTratamientos, "IdPlan", "IdPlan", pasoTratamiento.IdPlan);
            ViewData["IdTratamiento"] = new SelectList(_context.Tratamientos, "IdTratamiento", "Nombre", pasoTratamiento.IdTratamiento);
            return View(pasoTratamiento);
        }

        
        // Guarda los cambios realizados a un paso de tratamiento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdPaso,IdPlan,IdTratamiento,FechaEstimada,Estado,Observacion")] PasoTratamiento pasoTratamiento)
        {
            if (id != pasoTratamiento.IdPaso)
                return NotFound();

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
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdPlan"] = new SelectList(_context.PlanTratamientos, "IdPlan", "IdPlan", pasoTratamiento.IdPlan);
            ViewData["IdTratamiento"] = new SelectList(_context.Tratamientos, "IdTratamiento", "Nombre", pasoTratamiento.IdTratamiento);
            return View(pasoTratamiento);
        }

        
        // Muestra la vista para confirmar la eliminación de un paso de tratamiento
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
                return NotFound();

            var pasoTratamiento = await _context.PasoTratamientos
                .Include(p => p.Plan)
                .Include(p => p.Tratamiento)
                .FirstOrDefaultAsync(m => m.IdPaso == id);

            if (pasoTratamiento == null)
                return NotFound();

            return View(pasoTratamiento);
        }

        
        // Elimina un paso de tratamiento de forma definitiva
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var pasoTratamiento = await _context.PasoTratamientos.FindAsync(id);
            if (pasoTratamiento != null)
                _context.PasoTratamientos.Remove(pasoTratamiento);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Verifica si existe un paso de tratamiento con el ID indicado
        private bool PasoTratamientoExists(Guid id)
        {
            return _context.PasoTratamientos.Any(e => e.IdPaso == id);
        }
    }
}
