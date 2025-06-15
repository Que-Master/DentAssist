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
        [HttpGet]
        public IActionResult Index(string rol)
        {
            ViewBag.Rol = rol;
            return View();
        }

        // POST: /Login
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            var odontologo = _context.Odontologos
                .FirstOrDefault(o => o.Email == model.Email && o.Contrasenia == model.Contrasenia);

            if (odontologo == null)
            {
                ModelState.AddModelError("", "Correo o contraseña incorrectos.");
                return View();
            }

            HttpContext.Session.SetString("OdontologoId", odontologo.IdOdontologo.ToString());
            return RedirectToAction("Menu");
        }

    }
}
