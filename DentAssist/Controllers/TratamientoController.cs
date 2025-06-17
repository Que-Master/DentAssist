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
    public class TratamientoController : Controller
    {
        private readonly AppDbContext _context;

        public TratamientoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Tratamiento
        // Muestra la lista de tratamientos registrados
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tratamientos.ToListAsync());
        }

        // GET: Tratamiento/Details/5
        // Muestra los detalles de un tratamiento específico
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tratamiento = await _context.Tratamientos
                .FirstOrDefaultAsync(m => m.IdTratamiento == id);
            if (tratamiento == null)
            {
                return NotFound();
            }

            return View(tratamiento);
        }

        // GET: Tratamiento/Create
        // Muestra el formulario para crear un nuevo tratamiento
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tratamiento/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // Procesa el formulario para crear un nuevo tratamiento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTratamiento,Nombre,Descripcion,PrecioEstimado")] Tratamiento tratamiento)
        {
            if (ModelState.IsValid)
            {
                tratamiento.IdTratamiento = Guid.NewGuid();
                _context.Add(tratamiento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tratamiento); // Si hay errores, vuelve al formulario
        }

        // GET: Tratamiento/Edit/5
        // Muestra el formulario para editar un tratamiento existente
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tratamiento = await _context.Tratamientos.FindAsync(id);
            if (tratamiento == null)
            {
                return NotFound();
            }
            return View(tratamiento);
        }

        // POST: Tratamiento/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // Procesa la edición del tratamiento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdTratamiento,Nombre,Descripcion,PrecioEstimado")] Tratamiento tratamiento)
        {
            if (id != tratamiento.IdTratamiento)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tratamiento); // Actualiza los datos
                    await _context.SaveChangesAsync(); // Guarda los cambios
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TratamientoExists(tratamiento.IdTratamiento))
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
            return View(tratamiento); // Si hay errores, vuelve al formulario
        }

        // GET: Tratamiento/Delete/5
        // Muestra una vista para confirmar la eliminación de un tratamiento
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tratamiento = await _context.Tratamientos
                .FirstOrDefaultAsync(m => m.IdTratamiento == id);
            if (tratamiento == null)
            {
                return NotFound();
            }

            return View(tratamiento);
        }

        // POST: Tratamiento/Delete/5
        // Elimina definitivamente el tratamiento (tras la confirmación)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var tratamiento = await _context.Tratamientos.FindAsync(id);
            if (tratamiento != null)
            {
                _context.Tratamientos.Remove(tratamiento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Verifica si un tratamiento con ese ID existe
        private bool TratamientoExists(Guid id)
        {
            return _context.Tratamientos.Any(e => e.IdTratamiento == id);
        }
    }
}
