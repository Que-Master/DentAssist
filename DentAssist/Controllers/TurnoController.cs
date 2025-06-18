using System;
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

        // Muestra la lista de todos los turnos junto con sus odontólogos y pacientes
        public async Task<IActionResult> Index()
        {
            ViewData["OdontologoId"] = new SelectList(_context.Odontologos, "IdOdontologo", "Nombre");
            ViewBag.OdontologoId = new SelectList(_context.Odontologos, "IdOdontologo", "Nombre");

            var turnos = _context.Turnos
                .Include(t => t.Odontologo)
                .Include(t => t.Paciente);

            return View(await turnos.ToListAsync());
        }

        // Muestra los detalles de un turno específico
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
                return NotFound();

            var turno = await _context.Turnos
                .Include(t => t.Odontologo)
                .Include(t => t.Paciente)
                .FirstOrDefaultAsync(t => t.IdTurno == id);

            if (turno == null)
                return NotFound();

            return View(turno);
        }

        // Muestra los turnos asignados a un odontólogo específico
        public async Task<IActionResult> MisTurnos(Guid? idOdontologo)
        {
            if (idOdontologo == null)
                return BadRequest("Debes proporcionar un ID de odontólogo.");

            var turnos = await _context.Turnos
                .Include(t => t.Paciente)
                .Include(t => t.Odontologo)
                .Where(t => t.IdOdontologo == idOdontologo)
                .OrderBy(t => t.FechaHora)
                .ToListAsync();

            ViewData["NombreOdontologo"] = await _context.Odontologos
                .Where(o => o.IdOdontologo == idOdontologo)
                .Select(o => o.Nombre)
                .FirstOrDefaultAsync();

            return View("MisTurnos", turnos);
        }

        // Muestra los turnos a partir de hoy en adelante
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

        // Muestra el formulario para crear un nuevo turno
        public IActionResult Create()
        {
            ViewData["IdOdontologo"] = new SelectList(_context.Odontologos, "IdOdontologo", "Nombre");
            ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "IdPaciente", "Nombre");
            return View();
        }

        // Procesa el formulario de creación de un nuevo turno
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

        // Muestra el formulario para editar un turno existente
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
                return NotFound();

            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null)
                return NotFound();

            ViewData["IdOdontologo"] = new SelectList(_context.Odontologos, "IdOdontologo", "Nombre", turno.IdOdontologo);
            ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "IdPaciente", "Nombre", turno.IdPaciente);
            return View(turno);
        }

        // Procesa la edición de un turno
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdTurno,FechaHora,DuracionMinutos,Estado,IdPaciente,IdOdontologo")] Turno turno)
        {
            if (id != turno.IdTurno)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["IdOdontologo"] = new SelectList(_context.Odontologos, "IdOdontologo", "Nombre", turno.IdOdontologo);
                ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "IdPaciente", "Nombre", turno.IdPaciente);
                return View(turno);
            }

            try
            {
                _context.Update(turno);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TurnoExists(turno.IdTurno))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // Muestra la vista para confirmar eliminación de un turno
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
                return NotFound();

            var turno = await _context.Turnos
                .Include(t => t.Odontologo)
                .Include(t => t.Paciente)
                .FirstOrDefaultAsync(t => t.IdTurno == id);

            if (turno == null)
                return NotFound();

            return View(turno);
        }

        // Elimina un turno de forma definitiva
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var turno = await _context.Turnos.FindAsync(id);
            if (turno != null)
            {
                _context.Turnos.Remove(turno);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Verifica si un turno existe según su ID
        private bool TurnoExists(Guid id)
        {
            return _context.Turnos.Any(t => t.IdTurno == id);
        }
    }
}
