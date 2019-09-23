using System.Diagnostics;
using LisasTours.Data;
using LisasTours.Models;
using Microsoft.AspNetCore.Mvc;

namespace LisasTours.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewData["Regions"] = _context.Set<Region>();
            ViewData["BusinessLines"] = _context.Set<BusinessLine>();
            return View();
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
