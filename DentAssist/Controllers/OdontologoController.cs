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
    public class OdontologoController : Controller
    {
        private readonly AppDbContext _context;

        public OdontologoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Odontologo/Login
        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.Rol = "Odontólogo";
            return View("Login");
        }


        [HttpPost]
        public IActionResult Login(string email, string contrasenia)
        {
            var odontologo = _context.Odontologos
                .FirstOrDefault(o => o.Email == email && o.Contrasenia == contrasenia);

            if (odontologo == null)
            {
                ViewBag.Error = "Correo o contraseña incorrectos.";
                return View();
            }

            // Guardamos en sesión el ID del odontólogo
            HttpContext.Session.SetString("OdontologoId", odontologo.IdOdontologo.ToString());

            return RedirectToAction("Menu");
        }


        // GET: Odontologo/Menu
        public IActionResult Menu()
        {
            return View();
        }

        // GET: Odontologo
        public async Task<IActionResult> Index()
        {
            return View(await _context.Odontologos.ToListAsync());
        }

        // GET: Odontologo/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var odontologo = await _context.Odontologos
                .FirstOrDefaultAsync(m => m.IdOdontologo == id);
            if (odontologo == null)
            {
                return NotFound();
            }

            return View(odontologo);
        }

        // GET: Odontologo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Odontologo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdOdontologo,Nombre,Matricula,Especialidad,Email")] Odontologo odontologo)
        {
            if (ModelState.IsValid)
            {
                odontologo.IdOdontologo = Guid.NewGuid();
                _context.Add(odontologo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(odontologo);
        }

        // GET: Odontologo/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var odontologo = await _context.Odontologos.FindAsync(id);
            if (odontologo == null)
            {
                return NotFound();
            }
            return View(odontologo);
        }

        // POST: Odontologo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdOdontologo,Nombre,Matricula,Especialidad,Email")] Odontologo odontologo)
        {
            if (id != odontologo.IdOdontologo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(odontologo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OdontologoExists(odontologo.IdOdontologo))
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
            return View(odontologo);
        }

        // GET: Odontologo/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var odontologo = await _context.Odontologos
                .FirstOrDefaultAsync(m => m.IdOdontologo == id);
            if (odontologo == null)
            {
                return NotFound();
            }

            return View(odontologo);
        }

        // POST: Odontologo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var odontologo = await _context.Odontologos.FindAsync(id);
            if (odontologo != null)
            {
                _context.Odontologos.Remove(odontologo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OdontologoExists(Guid id)
        {
            return _context.Odontologos.Any(e => e.IdOdontologo == id);
        }
    }
}
