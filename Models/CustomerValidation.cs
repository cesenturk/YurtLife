
namespace YurtLife.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    public class CustomerValidation
    {
        [Required(ErrorMessage = "Boş Bırakılamaz.")]
        public string EmailAdress { get; set; }
        [Required(ErrorMessage = "Boş Bırakılamaz.")]
        public string Password { get; set; }
        
    }
}