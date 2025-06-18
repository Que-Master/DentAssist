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
    public class PacienteController : Controller
    {
        private readonly AppDbContext _context;

        public PacienteController(AppDbContext context)
        {
            _context = context;
        }

        
        // Muestra la lista de pacientes registrados
        public async Task<IActionResult> Index()
        {
            var pacientes = await _context.Pacientes.ToListAsync();
            return View(pacientes);
        }

        
        // Muestra los detalles de un paciente específico
        // Parámetro id: Id del paciente a buscar
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes
                .FirstOrDefaultAsync(m => m.IdPaciente == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        
        // Muestra el formulario para crear un nuevo paciente
        public IActionResult Create()
        {
            return View();
        }

        
        // Procesa el formulario para crear un nuevo paciente
        // Parámetro paciente: Datos ingresados en el formulario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPaciente,Nombre,RUT,Telefono,Email,Direccion")] Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                paciente.IdPaciente = Guid.NewGuid();
                _context.Add(paciente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(paciente);
        }

        
        // Muestra el formulario para editar un paciente existente
        // Parámetro id: Id del paciente a editar
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null)
            {
                return NotFound();
            }
            return View(paciente);
        }

        
        // Procesa la edición de un paciente
        // Parámetros:
        //   id: Id del paciente a modificar
        //   paciente: Datos modificados del paciente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdPaciente,Nombre,RUT,Telefono,Email,Direccion")] Paciente paciente)
        {
            if (id != paciente.IdPaciente)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paciente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PacienteExists(paciente.IdPaciente))
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
            return View(paciente);
        }

        
        // Muestra la confirmación para eliminar un paciente
        // Parámetro id: Id del paciente a eliminar
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes
                .FirstOrDefaultAsync(m => m.IdPaciente == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        
        // Elimina definitivamente el paciente después de confirmar
        // Parámetro id: Id del paciente a eliminar
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente != null)
            {
                _context.Pacientes.Remove(paciente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Verifica si un paciente con el Id especificado existe en la base de datos
        // Parámetro id: Id del paciente a verificar
        private bool PacienteExists(Guid id)
        {
            return _context.Pacientes.Any(e => e.IdPaciente == id);
        }
    }
}
