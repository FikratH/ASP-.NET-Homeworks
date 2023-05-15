using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Areas.Admin.ViewModels;
using Pronia.Models;
using Pronia.Utils;
using Pronia.Utils.Enums;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FeaturesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FeaturesController(AppDbContext context, IWebHostEnvironment webHostEnvironment = null)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FeaturesViewModel featureVM)
        {
            if (!ModelState.IsValid)
                return View();
            //if (slider.Offer < 0 || slider.Offer > 100)
            //{
            //    ModelState.AddModelError("Offer", "Offer value must be between 0 and 100.");
            //    return View();
            //}

            Features? foundFeature = _context.Features.FirstOrDefault(x => x.Title == featureVM.Title);
            if (foundFeature != null)
            {
                ModelState.AddModelError("Title", "This title already exists.");
                return View();
            }
            if (featureVM.Image == null)
            {
                ModelState.AddModelError("Image", "Please, upload an image!");
                return View();
            }
            if (featureVM.Image.CheckFileType(ContentType.image.ToString()))
            {
                ModelState.AddModelError("Image", "Please, upload an image!");
                return View();
            }
            if (!featureVM.Image.CheckFileSize(100))
            {
                ModelState.AddModelError("Image", "Image size can't exceed 100 kB");
                return View();
            }
            string fileName =  $"{Guid.NewGuid()}-{featureVM.Image.FileName}";
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "website-images", fileName);

            using (FileStream stream = new(path, FileMode.Create)){
                await featureVM.Image.CopyToAsync(stream);
            }

            Features feature = new()
            {
                Image = fileName,
                Title = featureVM.Title,
                Description = featureVM.Description,
            };
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
        [ValidateAntiForgeryToken]
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
        [ValidateAntiForgeryToken]
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
