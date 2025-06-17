using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DentAssist.Models.Data;
using DentAssist.Models.Entities;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace DentAssist.Controllers
{
    public class RecepcionistaController : Controller
    {
        private readonly AppDbContext _context;

        public RecepcionistaController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.Rol = "Recepcionista";
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string contrasenia)
        {
            HttpContext.Session.SetString("Rol", "Recepcionista");
            // Verifica que el email y contraseña correspondan a un recepcionista
            var recepcionista = _context.Recepcionistas
                .FirstOrDefault(r => r.Email == email && r.Contrasenia == contrasenia);

            if (recepcionista == null)
            {
                // Si no coincide, muestra mensaje de error
                ViewBag.Error = "Correo o contraseña incorrectos. Solo los recepcionistas pueden iniciar sesión.";
                return View();
            }

            // Guardar el ID del recepcionista en sesión
            HttpContext.Session.SetString("RecepcionistaId", recepcionista.Id.ToString());

            // Redirige al menú del recepcionista
            return RedirectToAction("Menu");
        }

        public IActionResult Menu()
        {
            return View();
        }


        // GET: Recepcionista
        // Muestra la lista de recepcionistas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Recepcionistas.ToListAsync());
        }

        // GET: Recepcionista/Details/5
        // Muestra los detalles de un recepcionista específico
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
        // Muestra el formulario para crear un nuevo recepcionista
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recepcionista/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // Procesa la creación de un nuevo recepcionista (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Email,Contrasenia")] Recepcionista recepcionista)
        {
            // Valida si el correo ya existe
            bool correoExiste = await _context.Recepcionistas.AnyAsync(r => r.Email == recepcionista.Email)
                             || await _context.Odontologos.AnyAsync(o => o.Email == recepcionista.Email);

            if (recepcionista.Email == "admin@dentassist.cl")
            {
                ModelState.AddModelError("Email", "Este correo ya está registrado en el sistema.");
                return View(recepcionista);
            }

            if (correoExiste)
            {
                ModelState.AddModelError("Email", "Este correo ya está registrado en el sistema.");
                return View(recepcionista);
            }

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
        // Muestra el formulario para editar los datos de un recepcionista
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
        // Procesa la edición de los datos del recepcionista (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Nombre,Email")] Recepcionista recepcionista)
        {
            if (id != recepcionista.Id)
            {
                return NotFound();
            }

            // Validar si ya existe un usuario con ese correo
            bool correoUsado = await _context.Recepcionistas
                .AnyAsync(r => r.Email == recepcionista.Email && r.Id != recepcionista.Id) ||
                await _context.Odontologos.AnyAsync(o => o.Email == recepcionista.Email) ||
                recepcionista.Email == "admin@dentassist.cl";

            if (correoUsado)
            {
                ModelState.AddModelError("Email", "Este correo ya está en uso por otro usuario.");
                return View(recepcionista);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Obtener el recepcionista original
                    var original = await _context.Recepcionistas.FindAsync(id);
                    if (original == null) return NotFound();

                    // Solo actualizar nombre y email
                    original.Nombre = recepcionista.Nombre;
                    original.Email = recepcionista.Email;

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
        // Muestra la vista de confirmación para eliminar un recepcionista
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
        // Elimina un recepcionista de la base de datos (POST)
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

        // Verifica si existe un recepcionista con el ID dado
        private bool RecepcionistaExists(Guid id)
        {
            return _context.Recepcionistas.Any(e => e.Id == id);
        }
    }
}
