using System.ComponentModel.DataAnnotations;

namespace Pronia.Models
{
    public class Slider
    {
        public int ID { get; set; }
        [MaxLength(100),Required]
        public string Title { get; set; }
        [MaxLength(200), Required]
        public string Description { get; set; }
        [Required]
        public int Offer { get; set; }

        public string Image { get; set; }
    }
}
