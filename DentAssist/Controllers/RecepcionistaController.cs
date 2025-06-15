using Microsoft.AspNetCore.Mvc;
using DentAssist.Models.Data;
using DentAssist.Models.Entities;
using System.Linq;

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
            return View("Login");
        }


        [HttpPost]
        public IActionResult Login(string email, string contrasenia)
        {
            var recepcionista = _context.Recepcionistas
                .FirstOrDefault(r => r.Email == email && r.Contrasenia == contrasenia);

            if (recepcionista == null)
            {
                ViewBag.Error = "Correo o contraseña incorrectos.";
                return View();
            }

            // (Opcional) Guardar en sesión
            HttpContext.Session.SetString("RecepcionistaId", recepcionista.Id.ToString());

            return RedirectToAction("Menu");
        }

        public IActionResult Menu()
        {
            return View();
        }
    }
}
