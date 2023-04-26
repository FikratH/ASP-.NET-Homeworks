using Microsoft.AspNetCore.Mvc;
using Pronia.Context;
using Pronia.Models;

namespace Pronia.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Slider> sliders = _context.Sliders.ToList();
            List<Features> features = _context.Features.ToList();

            ViewBag.Features = features;
            return View(sliders);
        }
    }
}
