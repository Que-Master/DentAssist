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

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var turnosProximos = _context.Turnos
                .Include(t => t.Paciente)
                .Include(t => t.Odontologo)
                .Where(t => t.FechaHora >= DateTime.Now)
                .OrderBy(t => t.FechaHora)
                .Take(10)
                .ToList();

            return View(turnosProximos);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
