using Microsoft.Build.Framework;

namespace Pronia.Areas.Admin.ViewModels
{
    public class FeaturesViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public IFormFile? Image { get; set; }
    }
}
