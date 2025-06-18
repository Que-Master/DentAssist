using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DentAssist.Models;
using DentAssist.Models.Data;
using DentAssist.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DentAssist.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        // Constructor que recibe el logger y el contexto de base de datos
        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Muestra la p�gina principal con los pr�ximos turnos agendados
        public IActionResult Index()
        {
            // Consulta los 10 turnos m�s pr�ximos con info de paciente y odont�logo
            var turnosProximos = _context.Turnos
                .Include(t => t.Paciente)
                .Include(t => t.Odontologo)
                .Where(t => t.FechaHora >= DateTime.Now)
                .OrderBy(t => t.FechaHora)
                .Take(10)
                .ToList();

            return View(turnosProximos);
        }

        // Muestra la vista de pol�ticas de privacidad
        public IActionResult Privacy()
        {
            return View();
        }

        // Muestra la vista de error en caso de fallas
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Limpia la sesi�n y redirige a la p�gina principal
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
