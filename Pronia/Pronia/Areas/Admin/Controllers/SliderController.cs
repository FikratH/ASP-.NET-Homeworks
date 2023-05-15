using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Pronia.Areas.Admin.ViewModels;
using Pronia.Models;
using Pronia.Utils;
using System.Drawing;
using System.IO;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SliderController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Slider> sliders = _context.Sliders.AsEnumerable();
            ViewBag.Count = sliders.Count();
            return View(sliders);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(SlidersViewModel sliderVM)
        {
            if (!ModelState.IsValid)
                return View();

            if (sliderVM.Image == null)
            {
                ModelState.AddModelError("Image", "Please, upload an image!");
                return View();
            }
            if (sliderVM.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "Please, upload an image!");
                return View();
            }
            if (!sliderVM.Image.CheckFileSize(1000))
            {
                ModelState.AddModelError("Image", "Image size can't exceed 1 megabyte");
                return View();
            }
            string fileName = $"{Guid.NewGuid()}-{sliderVM.Image.FileName}";
            string path = FileService.DefinePath(_webHostEnvironment.WebRootPath, "assets", "images", "website-images", fileName);

            using (FileStream stream = new(path, FileMode.Create))
            {
                await sliderVM.Image.CopyToAsync(stream);
            }

            Slider slider = new()
            {
                Image = fileName,
                Description = sliderVM.Description,
                Offer = sliderVM.Offer,
                Title = sliderVM.Title,
            };

            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Detail(int id)
        {

            Slider? slider = await _context.Sliders.AsNoTracking().FirstOrDefaultAsync(x => x.ID == id);
            if (slider == null)
                return NotFound();
            return View(slider);
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Sliders.Count() == 1)
            {
                return BadRequest();
            }
            Slider? slider = await _context.Sliders.FirstOrDefaultAsync(x => x.ID == id);
            if (slider == null)
                return NotFound();
            return View(slider);
        }
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSlider(int id)
        {
            Slider? slider = await _context.Sliders.FirstOrDefaultAsync(x => x.ID == id);
            if (slider == null)
                return NotFound();

            FileService.DeleteFile(_webHostEnvironment.WebRootPath, "assets", "images", "website-images", slider.Image);
            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            Slider? slider = await _context.Sliders.FirstOrDefaultAsync(x => x.ID == id);
            if (slider == null)
                return NotFound();

            SlidersViewModel sliderVM = new()
            {
                ID = slider.ID,
                Title = slider.Title,
                Description = slider.Description,
                Offer = slider.Offer
            };

            return View(sliderVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, SlidersViewModel sliderVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Slider? dbSlider = await _context.Sliders.FirstOrDefaultAsync(x => x.ID == id);
            if (dbSlider == null)
                return NotFound();

            if(sliderVM.Image != null) {
                if (sliderVM.Image.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Image", "Please, upload an image!");
                    return View();
                }
                if (!sliderVM.Image.CheckFileSize(1000))
                {
                    ModelState.AddModelError("Image", "Image size can't exceed 1 megabyte");
                    return View();
                }
                string path = FileService.DefinePath(_webHostEnvironment.WebRootPath, "assets", "images", "website-images", dbSlider.Image);
                FileService.DeleteFile(path);


                string fileName = $"{Guid.NewGuid()}-{sliderVM.Image.FileName}";
                string newPath = FileService.DefinePath(_webHostEnvironment.WebRootPath, "assets", "images", "website-images", fileName);

                using (FileStream stream = new(newPath, FileMode.Create))
                {
                    await sliderVM.Image.CopyToAsync(stream);
                }
                dbSlider.Image = fileName;
            }


            //Slider newSlider = new()
            //{
            //    ID = sliderVM.ID,
            //    Title = sliderVM.Title,
            //    Description = sliderVM.Description,

            //};    

            dbSlider.Title = sliderVM.Title;
            dbSlider.Description = sliderVM.Description;
            dbSlider.Offer = sliderVM.Offer;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
