﻿using System;
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

        // Muestra el formulario de inicio de sesión del odontólogo (GET)
        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.Rol = "Odontólogo";
            return View("Login");
        }

        // Procesa el login del odontólogo (POST), verifica credenciales y crea sesión
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

            // Guardar datos en sesión
            HttpContext.Session.SetString("OdontologoId", odontologo.IdOdontologo.ToString());
            HttpContext.Session.SetString("Rol", "Odontologo");

            // Redirige al menú principal
            return RedirectToAction("Menu");
        }

        // Muestra el menú principal del odontólogo
        public IActionResult Menu()
        {
            return View();
        }

        // Lista todos los odontólogos registrados
        public async Task<IActionResult> Index()
        {
            return View(await _context.Odontologos.ToListAsync());
        }

        // Muestra detalles de un odontólogo dado su ID
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
                return NotFound();

            var odontologo = await _context.Odontologos
                .FirstOrDefaultAsync(m => m.IdOdontologo == id);
            if (odontologo == null)
                return NotFound();

            return View(odontologo);
        }

        // Muestra formulario para crear un nuevo odontólogo
        public IActionResult Create()
        {
            return View();
        }

        // Procesa la creación de un odontólogo nuevo, valida email único
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdOdontologo,Nombre,Matricula,Especialidad,Email,Contrasenia")] Odontologo odontologo)
        {
            string adminEmail = "admin@dentassist.cl";

            // Verificar si el correo ya está usado (entre odontólogos, recepcionistas o admin)
            bool correoUsado = await _context.Odontologos.AnyAsync(o => o.Email == odontologo.Email)
                || await _context.Recepcionistas.AnyAsync(r => r.Email == odontologo.Email)
                || odontologo.Email == adminEmail;

            if (correoUsado)
            {
                ModelState.AddModelError("Email", "Este correo ya está registrado por otro usuario.");
                return View(odontologo);
            }

            if (ModelState.IsValid)
            {
                odontologo.IdOdontologo = Guid.NewGuid();
                _context.Add(odontologo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(odontologo);
        }

        // Muestra formulario para editar un odontólogo existente
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
                return NotFound();

            var odontologo = await _context.Odontologos.FindAsync(id);
            if (odontologo == null)
                return NotFound();

            return View(odontologo);
        }

        // Procesa la edición de un odontólogo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdOdontologo,Nombre,Matricula,Especialidad,Email")] Odontologo odontologo)
        {
            if (id != odontologo.IdOdontologo)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var odontologoOriginal = await _context.Odontologos.FindAsync(id);
                    if (odontologoOriginal == null)
                        return NotFound();

                    // Actualizar campos editables
                    odontologoOriginal.Nombre = odontologo.Nombre;
                    odontologoOriginal.Matricula = odontologo.Matricula;
                    odontologoOriginal.Especialidad = odontologo.Especialidad;
                    odontologoOriginal.Email = odontologo.Email;

                    _context.Update(odontologoOriginal);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OdontologoExists(odontologo.IdOdontologo))
                        return NotFound();
                    else
                        throw;
                }
            }
            return View(odontologo);
        }

        // Muestra vista para confirmar eliminación de un odontólogo
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
                return NotFound();

            var odontologo = await _context.Odontologos
                .FirstOrDefaultAsync(m => m.IdOdontologo == id);
            if (odontologo == null)
                return NotFound();

            return View(odontologo);
        }

        // Elimina un odontólogo (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var odontologo = await _context.Odontologos.FindAsync(id);
            if (odontologo != null)
            {
                _context.Odontologos.Remove(odontologo);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Verifica si un odontólogo existe por ID
        private bool OdontologoExists(Guid id)
        {
            return _context.Odontologos.Any(e => e.IdOdontologo == id);
        }
    }
}
