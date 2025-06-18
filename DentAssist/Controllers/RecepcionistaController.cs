using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        // Muestra el formulario de inicio de sesión para recepcionistas
        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.Rol = "Recepcionista";
            return View();
        }

        // Procesa el inicio de sesión del recepcionista
        [HttpPost]
        public IActionResult Login(string email, string contrasenia)
        {
            // Guarda el rol en sesión
            HttpContext.Session.SetString("Rol", "Recepcionista");

            // Busca el recepcionista con las credenciales proporcionadas
            var recepcionista = _context.Recepcionistas
                .FirstOrDefault(r => r.Email == email && r.Contrasenia == contrasenia);

            if (recepcionista == null)
            {
                ViewBag.Error = "Correo o contraseña incorrectos. Solo los recepcionistas pueden iniciar sesión.";
                return View();
            }

            // Guarda el ID del recepcionista en la sesión
            HttpContext.Session.SetString("RecepcionistaId", recepcionista.Id.ToString());

            // Redirige al menú del recepcionista
            return RedirectToAction("Menu");
        }

        // Muestra el menú principal del recepcionista después del login
        public IActionResult Menu()
        {
            return View();
        }

        // Muestra una lista de todos los recepcionistas registrados
        public async Task<IActionResult> Index()
        {
            return View(await _context.Recepcionistas.ToListAsync());
        }

        // Muestra los detalles de un recepcionista específico
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
                return NotFound();

            var recepcionista = await _context.Recepcionistas
                .FirstOrDefaultAsync(m => m.Id == id);

            if (recepcionista == null)
                return NotFound();

            return View(recepcionista);
        }

        // Muestra el formulario para registrar un nuevo recepcionista
        public IActionResult Create()
        {
            return View();
        }

        // Procesa la creación de un nuevo recepcionista
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Email,Contrasenia")] Recepcionista recepcionista)
        {
            // Verifica si el correo ya está registrado por otro usuario
            bool correoExiste = await _context.Recepcionistas.AnyAsync(r => r.Email == recepcionista.Email)
                             || await _context.Odontologos.AnyAsync(o => o.Email == recepcionista.Email);

            if (recepcionista.Email == "admin@dentassist.cl" || correoExiste)
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

        // Muestra el formulario para editar un recepcionista existente
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
                return NotFound();

            var recepcionista = await _context.Recepcionistas.FindAsync(id);

            if (recepcionista == null)
                return NotFound();

            return View(recepcionista);
        }

        // Procesa los cambios realizados a un recepcionista
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Nombre,Email")] Recepcionista recepcionista)
        {
            if (id != recepcionista.Id)
                return NotFound();

            // Verifica que el nuevo correo no esté usado por otro usuario
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
                    var original = await _context.Recepcionistas.FindAsync(id);
                    if (original == null)
                        return NotFound();

                    // Actualiza solo los campos permitidos
                    original.Nombre = recepcionista.Nombre;
                    original.Email = recepcionista.Email;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecepcionistaExists(recepcionista.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(recepcionista);
        }

        // Muestra una vista para confirmar la eliminación del recepcionista
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
                return NotFound();

            var recepcionista = await _context.Recepcionistas
                .FirstOrDefaultAsync(m => m.Id == id);

            if (recepcionista == null)
                return NotFound();

            return View(recepcionista);
        }

        // Procesa la eliminación del recepcionista
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var recepcionista = await _context.Recepcionistas.FindAsync(id);

            if (recepcionista != null)
                _context.Recepcionistas.Remove(recepcionista);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Verifica si existe un recepcionista con ese ID
        private bool RecepcionistaExists(Guid id)
        {
            return _context.Recepcionistas.Any(e => e.Id == id);
        }
    }
}
