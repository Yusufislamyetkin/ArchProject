using System.ComponentModel.DataAnnotations;

namespace Arch.UI.Models
{
    public class CardModel
    {
        [Required(ErrorMessage = "İsim gereklidir.")]
        [Display(Name = "İsim")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Kart numarası gereklidir.")]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Geçerli bir kart numarası giriniz.")]
        [Display(Name = "Kart Numarası")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Son kullanma tarihi gereklidir.")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/?([0-9]{2})$", ErrorMessage = "Geçerli bir son kullanma tarihi giriniz. (MM/YY)")]
        [Display(Name = "Son Kullanma Tarihi")]
        public string ExpirationDate { get; set; }

        [Required(ErrorMessage = "Güvenlik kodu gereklidir.")]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "Geçerli bir güvenlik kodu giriniz.")]
        [Display(Name = "Güvenlik Kodu")]
        public string SecurityCode { get; set; }
    }
}
