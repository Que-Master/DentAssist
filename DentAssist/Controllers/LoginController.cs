using Microsoft.AspNetCore.Mvc;
using DentAssist.Models.Data;
using DentAssist.Models.ViewModels;

namespace DentAssist.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        // Muestra la vista de login según el rol proporcionado por parámetro
        [HttpGet]
        public IActionResult Index(string rol)
        {
            ViewBag.Rol = rol;
            return View();
        }

        // Muestra el formulario de inicio de sesión
        [HttpGet]
        public IActionResult Login() => View();

        // Procesa el formulario de inicio de sesión
        // Parámetro model: datos de login enviados desde el formulario
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Busca al odontólogo con el correo y contraseña ingresados
            var odontologo = _context.Odontologos
                .FirstOrDefault(o => o.Email == model.Email && o.Contrasenia == model.Contrasenia);

            if (odontologo == null)
            {
                ModelState.AddModelError("", "Correo o contraseña incorrectos.");
                return View(model);
            }

            // Guarda el ID del odontólogo en la sesión
            HttpContext.Session.SetString("IdOdontologo", odontologo.IdOdontologo.ToString());

            // Redirige al menú principal (se asume que existe esta acción en otro controlador)
            return RedirectToAction("Menu");
        }
    }
}
