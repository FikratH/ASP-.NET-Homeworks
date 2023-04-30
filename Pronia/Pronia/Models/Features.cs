using System.ComponentModel.DataAnnotations;

namespace Pronia.Models
{
    public class Features
    {
        public int ID { get; set; }
        public string Image { get; set; }
        [MaxLength(20), Required]
        public string Title { get; set; }
        [MaxLength(100), Required]
        public string Description { get; set; }
    }
}
