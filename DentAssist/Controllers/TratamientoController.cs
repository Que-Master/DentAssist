using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DentAssist.Models.Data;
using DentAssist.Models.Entities;

namespace DentAssist.Controllers
{
    public class TratamientoController : Controller
    {
        private readonly AppDbContext _context;

        public TratamientoController(AppDbContext context)
        {
            _context = context;
        }

        // Muestra la lista de tratamientos registrados
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tratamientos.ToListAsync());
        }

        // Muestra los detalles de un tratamiento específico
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
                return NotFound();

            var tratamiento = await _context.Tratamientos
                .FirstOrDefaultAsync(m => m.IdTratamiento == id);

            if (tratamiento == null)
                return NotFound();

            return View(tratamiento);
        }

        // Muestra el formulario para crear un nuevo tratamiento
        public IActionResult Create()
        {
            return View();
        }

        // Procesa el formulario para crear un nuevo tratamiento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTratamiento,Nombre,Descripcion,PrecioEstimado")] Tratamiento tratamiento)
        {
            if (!ModelState.IsValid)
                return View(tratamiento);

            tratamiento.IdTratamiento = Guid.NewGuid();
            _context.Add(tratamiento);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Muestra el formulario para editar un tratamiento
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
                return NotFound();

            var tratamiento = await _context.Tratamientos.FindAsync(id);

            if (tratamiento == null)
                return NotFound();

            return View(tratamiento);
        }

        // Procesa la edición del tratamiento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdTratamiento,Nombre,Descripcion,PrecioEstimado")] Tratamiento tratamiento)
        {
            if (id != tratamiento.IdTratamiento)
                return NotFound();

            if (!ModelState.IsValid)
                return View(tratamiento);

            try
            {
                _context.Update(tratamiento);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TratamientoExists(tratamiento.IdTratamiento))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // Muestra la vista de confirmación para eliminar un tratamiento
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
                return NotFound();

            var tratamiento = await _context.Tratamientos
                .FirstOrDefaultAsync(m => m.IdTratamiento == id);

            if (tratamiento == null)
                return NotFound();

            return View(tratamiento);
        }

        // Elimina el tratamiento confirmado
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var tratamiento = await _context.Tratamientos.FindAsync(id);

            if (tratamiento != null)
                _context.Tratamientos.Remove(tratamiento);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Verifica si un tratamiento con el ID dado existe
        private bool TratamientoExists(Guid id)
        {
            return _context.Tratamientos.Any(e => e.IdTratamiento == id);
        }
    }
}
