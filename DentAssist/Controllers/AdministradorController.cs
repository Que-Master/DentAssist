using Microsoft.AspNetCore.Mvc;

namespace DentAssist.Controllers
{
    public class AdministradorController : Controller
    {
        private const string AdminEmail = "admin@dentassist.cl";
        private const string AdminContrasenia = "admin123";

        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.Rol = "Administrador";
            return View("~/Views/Shared/Login.cshtml");
        }


        [HttpPost]
        [HttpPost]
        public IActionResult Login(string email, string contrasenia)
        {
            if (email == AdminEmail && contrasenia == AdminContrasenia)
            {
                return RedirectToAction("Menu");
            }

            ViewBag.Rol = "Administrador";
            ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");
            return View("~/Views/Shared/Login.cshtml");
        }


        public IActionResult Menu()
        {
            return View();
        }
    }
}
