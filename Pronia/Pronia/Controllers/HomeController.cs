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
            HomeVM homeViewModel = new()
            {
                Sliders = sliders,
                Features = features
            };
            return View(homeViewModel);
        }
    }
}
