using System.ComponentModel.DataAnnotations;

namespace Pronia.Areas.Admin.ViewModels
{
    public class SlidersViewModel
    {
        public int ID { get; set; }
        [MaxLength(100), Required]
        public string Title { get; set; }
        [MaxLength(200), Required]
        public string Description { get; set; }
        [Required]
        [Range(0,100)]
        public int Offer { get; set; }
        public IFormFile? Image { get; set; }
    }
}
