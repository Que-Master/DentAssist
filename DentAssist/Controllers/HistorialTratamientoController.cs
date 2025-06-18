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

        // Muestra la lista de todos los historiales de tratamiento con sus datos relacionados
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.HistorialTratamientos
                .Include(h => h.Odontologo)
                .Include(h => h.Paciente)
                .Include(h => h.PasoTratamiento)
                .Include(h => h.Tratamiento);
            return View(await appDbContext.ToListAsync());
        }

        // Muestra los detalles de un historial de tratamiento específico identificado por id
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

        // Muestra el formulario para crear un nuevo historial de tratamiento
        public IActionResult Create()
        {
            // Carga los datos necesarios para los dropdowns en el formulario
            ViewData["IdOdontologo"] = new SelectList(_context.Odontologos, "IdOdontologo", "Matricula");
            ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "IdPaciente", "Nombre");
            ViewData["IdPasoTratamiento"] = new SelectList(_context.PasoTratamientos, "IdPaso", "Estado");
            ViewData["IdTratamiento"] = new SelectList(_context.Tratamientos, "IdTratamiento", "Nombre");
            return View();
        }

        // Procesa el formulario para crear un historial de tratamiento nuevo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdHistorial,IdPaciente,IdTratamiento,IdOdontologo,FechaRealizada,Observacion,IdPasoTratamiento")] HistorialTratamiento historialTratamiento)
        {
            if (ModelState.IsValid)
            {
                // Genera un nuevo Id para el historial
                historialTratamiento.IdHistorial = Guid.NewGuid();

                // Añade el nuevo historial y guarda cambios en BD
                _context.Add(historialTratamiento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Si el modelo no es válido, recarga los dropdowns y muestra el formulario con errores
            ViewData["IdOdontologo"] = new SelectList(_context.Odontologos, "IdOdontologo", "Matricula", historialTratamiento.IdOdontologo);
            ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "IdPaciente", "Nombre", historialTratamiento.IdPaciente);
            ViewData["IdPasoTratamiento"] = new SelectList(_context.PasoTratamientos, "IdPaso", "Estado", historialTratamiento.IdPasoTratamiento);
            ViewData["IdTratamiento"] = new SelectList(_context.Tratamientos, "IdTratamiento", "Nombre", historialTratamiento.IdTratamiento);
            return View(historialTratamiento);
        }

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

            // Carga los datos para los dropdowns con los valores actuales seleccionados
            ViewData["IdOdontologo"] = new SelectList(_context.Odontologos, "IdOdontologo", "Matricula", historialTratamiento.IdOdontologo);
            ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "IdPaciente", "Nombre", historialTratamiento.IdPaciente);
            ViewData["IdPasoTratamiento"] = new SelectList(_context.PasoTratamientos, "IdPaso", "Estado", historialTratamiento.IdPasoTratamiento);
            ViewData["IdTratamiento"] = new SelectList(_context.Tratamientos, "IdTratamiento", "Nombre", historialTratamiento.IdTratamiento);
            return View(historialTratamiento);
        }

        // Procesa la edición del historial de tratamiento
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
                    // Actualiza el historial y guarda cambios en BD
                    _context.Update(historialTratamiento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Si no existe, retorna NotFound
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

            // Si modelo no válido, recarga dropdowns y muestra formulario con errores
            ViewData["IdOdontologo"] = new SelectList(_context.Odontologos, "IdOdontologo", "Matricula", historialTratamiento.IdOdontologo);
            ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "IdPaciente", "Nombre", historialTratamiento.IdPaciente);
            ViewData["IdPasoTratamiento"] = new SelectList(_context.PasoTratamientos, "IdPaso", "Estado", historialTratamiento.IdPasoTratamiento);
            ViewData["IdTratamiento"] = new SelectList(_context.Tratamientos, "IdTratamiento", "Nombre", historialTratamiento.IdTratamiento);
            return View(historialTratamiento);
        }

        // Muestra la vista de confirmación para eliminar un historial de tratamiento
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

        // Elimina definitivamente el historial de tratamiento tras confirmación
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

        // Método auxiliar que verifica si un historial con ese ID existe en la base de datos
        private bool HistorialTratamientoExists(Guid id)
        {
            return _context.HistorialTratamientos.Any(e => e.IdHistorial == id);
        }
    }
}