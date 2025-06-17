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

        // GET: /Login
        // Muestra la vista de login segun el rol proporcionado por parametro
        [HttpGet]
        public IActionResult Index(string rol)
        {
            ViewBag.Rol = rol;
            return View();
        }

        // Muestra el formulario de inicio de sesion (GET)
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var odontologo = _context.Odontologos
                .FirstOrDefault(o => o.Email == model.Email && o.Contrasenia == model.Contrasenia);

            if (odontologo == null)
            {
                ModelState.AddModelError("", "Correo o contraseña incorrectos.");
                return View(model);
            }

            // Guarda el ID del odontólogo en la sesión
            HttpContext.Session.SetString("IdOdontologo", odontologo.IdOdontologo.ToString());

            return RedirectToAction("Menu");
        }


    }
}
