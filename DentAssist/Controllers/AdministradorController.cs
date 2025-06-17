using Microsoft.AspNetCore.Mvc;

namespace DentAssist.Controllers
{
    public class AdministradorController : Controller
    {
        // Credenciales fijas del administrador
        private const string AdminEmail = "admin@dentassist.cl";
        private const string AdminContrasenia = "admin123";

        // Muestra el formulario de inicio de sesion del administrador
        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.Rol = "Administrador";
            return View("~/Views/Shared/Login.cshtml");
        }

        // Procesa el inicio de sesion del administrador
        [HttpPost]
        public IActionResult Login(string email, string contrasenia)
        {
            HttpContext.Session.SetString("Rol", "Administrador");
            // Verifica si las credenciales son correctas
            if (email == AdminEmail && contrasenia == AdminContrasenia)
            {
                return RedirectToAction("Menu");
            }

            //Si son incorrectas, se muestra un mensaje de error
            ViewBag.Rol = "Administrador";
            ViewBag.Error = "Correo o contraseña incorrectos.";
            return View("~/Views/Shared/Login.cshtml");
        }

        // Muestra el menu principal del administrador
        public IActionResult Menu()
        {
            return View();
        }
    }
}
