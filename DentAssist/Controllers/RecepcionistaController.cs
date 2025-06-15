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
    public class RecepcionistaController : Controller
    {
        private readonly AppDbContext _context;

        public RecepcionistaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Recepcionista
        public async Task<IActionResult> Index()
        {
            return View(await _context.Recepcionistas.ToListAsync());
        }

        // GET: Recepcionista/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recepcionista = await _context.Recepcionistas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recepcionista == null)
            {
                return NotFound();
            }

            return View(recepcionista);
        }

        // GET: Recepcionista/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recepcionista/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Email,Contraseña")] Recepcionista recepcionista)
        {
            if (ModelState.IsValid)
            {
                recepcionista.Id = Guid.NewGuid();
                _context.Add(recepcionista);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recepcionista);
        }

        // GET: Recepcionista/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recepcionista = await _context.Recepcionistas.FindAsync(id);
            if (recepcionista == null)
            {
                return NotFound();
            }
            return View(recepcionista);
        }

        // POST: Recepcionista/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Nombre,Email,Contraseña")] Recepcionista recepcionista)
        {
            if (id != recepcionista.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recepcionista);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecepcionistaExists(recepcionista.Id))
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
            return View(recepcionista);
        }

        // GET: Recepcionista/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recepcionista = await _context.Recepcionistas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recepcionista == null)
            {
                return NotFound();
            }

            return View(recepcionista);
        }

        // POST: Recepcionista/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var recepcionista = await _context.Recepcionistas.FindAsync(id);
            if (recepcionista != null)
            {
                _context.Recepcionistas.Remove(recepcionista);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecepcionistaExists(Guid id)
        {
            return _context.Recepcionistas.Any(e => e.Id == id);
        }
    }
}
