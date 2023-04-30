using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FeaturesController : Controller
    {
        private readonly AppDbContext _context;
        public FeaturesController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Features> features = _context.Features.ToList();
            ViewBag.Count = features.Count;
            return View(features);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Features feature)
        {
            if (!ModelState.IsValid)
                return View();
            //if (slider.Offer < 0 || slider.Offer > 100)
            //{
            //    ModelState.AddModelError("Offer", "Offer value must be between 0 and 100.");
            //    return View();
            //}

            Features? foundFeature = _context.Features.FirstOrDefault(x => x.Title == feature.Title);
            if (foundFeature != null)
            {
                ModelState.AddModelError("Title", "This title already exists.");
                return View();
            }
            _context.Features.Add(feature);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Detail(int id)
        {

            Features? feature = _context.Features.AsNoTracking().FirstOrDefault(x => x.ID == id);
            if (feature == null)
                return NotFound();
            return View(feature);
        }
        public IActionResult Delete(int id)
        {
            Features? feature = _context.Features.FirstOrDefault(x => x.ID == id);
            if (feature == null)
                return NotFound();
            return View(feature);
        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteFeature(int id)
        {
            Features? feature = _context.Features.FirstOrDefault(x => x.ID == id);
            if (feature == null)
                return NotFound();
            _context.Features.Remove(feature);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int id)
        {
            Features? feature = _context.Features.FirstOrDefault(x => x.ID == id);
            if (feature == null)
                return NotFound();

            return View(feature);
        }
        [HttpPost]
        public IActionResult Update(Features feature, int id)
        {
            Features? dbFeature = _context.Features.AsNoTracking().FirstOrDefault(x => x.ID == id);
            Features? foundFeature = _context.Features.FirstOrDefault(x => x.Title == feature.Title);
            if (dbFeature == null)
                return NotFound();
            if (foundFeature != null)
            {
                ModelState.AddModelError("Title", "This title already exists.");
                return View();
            }

            _context.Features.Update(feature);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
