
namespace YurtLife.Models
{
    using System.ComponentModel.DataAnnotations;
    public class UserCardInfo
    {
        [Required(ErrorMessage = "Zorunlu Alan"), StringLength(16, MinimumLength =16, ErrorMessage ="Geçersiz Kart Numarası")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Zorunlu Alan"), StringLength(3, MinimumLength = 3, ErrorMessage = "Geçersiz Güvenlik Numarası")]
        public string SecurityNumber { get; set; }

        [Required(ErrorMessage = "Zorunlu Alan")]
        public string CardHasName { get; set; }

        [Required(ErrorMessage = "Zorunlu Alan")]
        public int ExpYear { get; set; }

        [Required(ErrorMessage = "Zorunlu Alan")]
        public int ExpMonth { get; set; }
        
        [Required(ErrorMessage = "Zorunlu Alan")]
        public Product Product { get; set; }
        public int Id { get; set; }
    }
}