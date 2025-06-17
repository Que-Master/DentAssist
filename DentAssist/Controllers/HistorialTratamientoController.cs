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
    public class HistorialTratamientoController : Controller
    {
        private readonly AppDbContext _context;

        public HistorialTratamientoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: HistorialTratamiento
        // Muestra la lista de todos los historiales de tratamiento
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.HistorialTratamientos
                .Include(h => h.Odontologo)
                .Include(h => h.Paciente)
                .Include(h => h.PasoTratamiento)
                .Include(h => h.Tratamiento);
            return View(await appDbContext.ToListAsync());
        }

        // GET: HistorialTratamiento/Details/5
        // Muestra los detalles de un historial de tratamiento especifico
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historialTratamiento = await _context.HistorialTratamientos
                .Include(h => h.Odontologo)
                .Include(h => h.Paciente)
                .Include(h => h.PasoTratamiento)
                .Include(h => h.Tratamiento)
                .FirstOrDefaultAsync(m => m.IdHistorial == id);
            if (historialTratamiento == null)
            {
                return NotFound();
            }

            return View(historialTratamiento);
        }

        // GET: HistorialTratamiento/Create
        // Muestra el formulario para crear un nuevo historial de tratamiento
        public IActionResult Create()
        {
            ViewData["IdOdontologo"] = new SelectList(_context.Odontologos, "IdOdontologo", "Matricula");
            ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "IdPaciente", "Nombre");
            ViewData["IdPasoTratamiento"] = new SelectList(_context.PasoTratamientos, "IdPaso", "Estado");
            ViewData["IdTratamiento"] = new SelectList(_context.Tratamientos, "IdTratamiento", "Nombre");
            return View();
        }

        // POST: HistorialTratamiento/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // Procesa el formulario de creacion de historial de tratamiento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdHistorial,IdPaciente,IdTratamiento,IdOdontologo,FechaRealizada,Observacion,IdPasoTratamiento")] HistorialTratamiento historialTratamiento)
        {
            if (ModelState.IsValid)
            {
                historialTratamiento.IdHistorial = Guid.NewGuid();
                _context.Add(historialTratamiento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdOdontologo"] = new SelectList(_context.Odontologos, "IdOdontologo", "Matricula", historialTratamiento.IdOdontologo);
            ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "IdPaciente", "Nombre", historialTratamiento.IdPaciente);
            ViewData["IdPasoTratamiento"] = new SelectList(_context.PasoTratamientos, "IdPaso", "Estado", historialTratamiento.IdPasoTratamiento);
            ViewData["IdTratamiento"] = new SelectList(_context.Tratamientos, "IdTratamiento", "Nombre", historialTratamiento.IdTratamiento);
            return View(historialTratamiento);
        }

        // GET: HistorialTratamiento/Edit/5
        // Muestra el formulario para editar un historial de tratamiento existente
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historialTratamiento = await _context.HistorialTratamientos.FindAsync(id);
            if (historialTratamiento == null)
            {
                return NotFound();
            }
            ViewData["IdOdontologo"] = new SelectList(_context.Odontologos, "IdOdontologo", "Matricula", historialTratamiento.IdOdontologo);
            ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "IdPaciente", "Nombre", historialTratamiento.IdPaciente);
            ViewData["IdPasoTratamiento"] = new SelectList(_context.PasoTratamientos, "IdPaso", "Estado", historialTratamiento.IdPasoTratamiento);
            ViewData["IdTratamiento"] = new SelectList(_context.Tratamientos, "IdTratamiento", "Nombre", historialTratamiento.IdTratamiento);
            return View(historialTratamiento);
        }

        // POST: HistorialTratamiento/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // Procesa la edicion de un historial de tratamiento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdHistorial,IdPaciente,IdTratamiento,IdOdontologo,FechaRealizada,Observacion,IdPasoTratamiento")] HistorialTratamiento historialTratamiento)
        {
            if (id != historialTratamiento.IdHistorial)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(historialTratamiento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HistorialTratamientoExists(historialTratamiento.IdHistorial))
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
            ViewData["IdOdontologo"] = new SelectList(_context.Odontologos, "IdOdontologo", "Matricula", historialTratamiento.IdOdontologo);
            ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "IdPaciente", "Nombre", historialTratamiento.IdPaciente);
            ViewData["IdPasoTratamiento"] = new SelectList(_context.PasoTratamientos, "IdPaso", "Estado", historialTratamiento.IdPasoTratamiento);
            ViewData["IdTratamiento"] = new SelectList(_context.Tratamientos, "IdTratamiento", "Nombre", historialTratamiento.IdTratamiento);
            return View(historialTratamiento);
        }

        // GET: HistorialTratamiento/Delete/5
        // Muestra la vista de confirmacion para eliminar un historial de tratamiento
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historialTratamiento = await _context.HistorialTratamientos
                .Include(h => h.Odontologo)
                .Include(h => h.Paciente)
                .Include(h => h.PasoTratamiento)
                .Include(h => h.Tratamiento)
                .FirstOrDefaultAsync(m => m.IdHistorial == id);
            if (historialTratamiento == null)
            {
                return NotFound();
            }

            return View(historialTratamiento);
        }

        // POST: HistorialTratamiento/Delete/5
        // Elimina definitivamente el historial de tratamiento (tras la confirmacion)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var historialTratamiento = await _context.HistorialTratamientos.FindAsync(id);
            if (historialTratamiento != null)
            {
                _context.HistorialTratamientos.Remove(historialTratamiento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // Verifica si un historial de tratamiento con ese ID existe
        private bool HistorialTratamientoExists(Guid id)
        {
            return _context.HistorialTratamientos.Any(e => e.IdHistorial == id);
        }
    }
}
