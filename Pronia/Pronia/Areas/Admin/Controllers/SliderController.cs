using Microsoft.EntityFrameworkCore;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;

        public SliderController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Slider> sliders = _context.Sliders;
            ViewBag.Count = sliders.Count();
            return View(sliders);
        }

        public IActionResult Create()
        {   
            return View();
        }
        [HttpPost]
        public IActionResult Create(Slider slider)
        {
            if (!ModelState.IsValid)
                return View();

            if (slider.Offer < 0 || slider.Offer > 100)
            {
                ModelState.AddModelError("Offer", "Offer value must be between 0 and 100.");
                return View();
            }
            _context.Sliders.Add(slider);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Detail(int id)
        {

            Slider? slider = _context.Sliders.AsNoTracking().FirstOrDefault(x => x.ID == id);
            if (slider == null)
                return NotFound();
            return View(slider);
        }
        public IActionResult Delete(int id)
        {
            if (_context.Sliders.Count() == 1)
            {
                return BadRequest();
            }
            Slider? slider = _context.Sliders.FirstOrDefault(x => x.ID == id);
            if (slider == null)
                return NotFound();
            return View(slider);
        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteSlider(int id)
        {
            Slider? slider = _context.Sliders.FirstOrDefault(x => x.ID == id);
            if (slider == null)
                return NotFound();
            _context.Sliders.Remove(slider);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int id)
        {
            Slider? slider = _context.Sliders.FirstOrDefault(x => x.ID == id);
            if (slider == null)
                return NotFound();

            return View(slider);
        }

        [HttpPost]
        public IActionResult Update(Slider slider, int id)
        {
            Slider? dbSlider = _context.Sliders.AsNoTracking().FirstOrDefault(x => x.ID == id);
            if (dbSlider == null)
                return NotFound();

            _context.Sliders.Update(slider);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
