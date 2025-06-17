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
    public class TurnoController : Controller
    {
        private readonly AppDbContext _context;

        public TurnoController(AppDbContext context)
        {
            _context = context;
        }


        // GET: Turno
        // Muestra la lista de todos los turnos con sus odontólogos y pacientes
        public async Task<IActionResult> Index()
        {
            ViewData["OdontologoId"] = new SelectList(_context.Odontologos, "IdOdontologo", "Nombre");
            ViewBag.OdontologoId = new SelectList(_context.Odontologos, "IdOdontologo", "Nombre");

            var appDbContext = _context.Turnos.Include(t => t.Odontologo).Include(t => t.Paciente);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Turno/Details/5
        // Muestra los detalles de un turno específico
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turno = await _context.Turnos
                .Include(t => t.Odontologo)
                .Include(t => t.Paciente)
                .FirstOrDefaultAsync(m => m.IdTurno == id);
            if (turno == null)
            {
                return NotFound();
            }

            return View(turno);
        }

        // Muestra los turnos asignados a un odontólogo específico
        public async Task<IActionResult> MisTurnos(Guid? idOdontologo)
        {
            if (idOdontologo == null)
            {
                return BadRequest("Debes proporcionar un ID de odontólogo.");
            }

            var turnos = await _context.Turnos
                .Include(t => t.Paciente)
                .Include(t => t.Odontologo)
                .Where(t => t.IdOdontologo == idOdontologo)
                .OrderBy(t => t.FechaHora)
                .ToListAsync();

            ViewData["NombreOdontologo"] = _context.Odontologos
                .Where(o => o.IdOdontologo == idOdontologo)
                .Select(o => o.Nombre)
                .FirstOrDefault();

            return View("MisTurnos", turnos);
        }


        public async Task<IActionResult> TurnosProximos()
        {
            var hoy = DateTime.Today;
            var turnos = await _context.Turnos
                .Include(t => t.Odontologo)
                .Include(t => t.Paciente)
                .Where(t => t.FechaHora >= hoy)
                .OrderBy(t => t.FechaHora)
                .ToListAsync();

            return View(turnos);
        }


        // GET: Turno/Create
        // Muestra el formulario para registrar un nuevo turno
        public IActionResult Create()
        {
            ViewData["IdOdontologo"] = new SelectList(_context.Odontologos, "IdOdontologo", "Nombre");
            ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "IdPaciente", "Nombre");
            return View();
        }


        // POST: Turno/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // Procesa el registro de un nuevo turno (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTurno,FechaHora,DuracionMinutos,Estado,IdPaciente,IdOdontologo")] Turno turno)
        {
            var pacienteExiste = await _context.Pacientes.AnyAsync(p => p.IdPaciente == turno.IdPaciente);
            var odontologoExiste = await _context.Odontologos.AnyAsync(o => o.IdOdontologo == turno.IdOdontologo);

            if (!pacienteExiste)
                ModelState.AddModelError("IdPaciente", "El paciente seleccionado no existe.");

            if (!odontologoExiste)
                ModelState.AddModelError("IdOdontologo", "El odontólogo seleccionado no existe.");

            if (!ModelState.IsValid)
            {
                ViewData["IdOdontologo"] = new SelectList(_context.Odontologos, "IdOdontologo", "Nombre", turno.IdOdontologo);
                ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "IdPaciente", "Nombre", turno.IdPaciente);
                return View(turno);
            }

            turno.IdTurno = Guid.NewGuid();
            _context.Add(turno);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: Turno/Edit/5
        // Muestra el formulario para editar un turno existente
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null)
            {
                return NotFound();
            }
            ViewData["IdOdontologo"] = new SelectList(_context.Odontologos, "IdOdontologo", "Nombre", turno.IdOdontologo);
            ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "IdPaciente", "Nombre", turno.IdPaciente);
            return View(turno);
        }

        // POST: Turno/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // Procesa la edición del turno (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdTurno,FechaHora,DuracionMinutos,Estado,IdPaciente,IdOdontologo")] Turno turno)
        {
            if (id != turno.IdTurno)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(turno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TurnoExists(turno.IdTurno))
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

            ViewData["IdOdontologo"] = new SelectList(_context.Odontologos, "IdOdontologo", "Nombre", turno.IdOdontologo);
            ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "IdPaciente", "Nombre", turno.IdPaciente);
            return View(turno);
        }

        // GET: Turno/Delete/5
        // Muestra la vista para confirmar la eliminación de un turno
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turno = await _context.Turnos
                .Include(t => t.Odontologo)
                .Include(t => t.Paciente)
                .FirstOrDefaultAsync(m => m.IdTurno == id);
            if (turno == null)
            {
                return NotFound();
            }

            return View(turno);
        }

        // POST: Turno/Delete/5
        // Elimina un turno de la base de datos (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var turno = await _context.Turnos.FindAsync(id);
            if (turno != null)
            {
                _context.Turnos.Remove(turno);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Verifica si un turno existe por su ID
        private bool TurnoExists(Guid id)
        {
            return _context.Turnos.Any(e => e.IdTurno == id);
        }
    }
}