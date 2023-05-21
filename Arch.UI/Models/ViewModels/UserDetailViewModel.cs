using System.ComponentModel.DataAnnotations;
using static Arch.EntityLayer.Entities.Auth.Authorization;

namespace Arch.UI.Models.ViewModels
{
    public class UserDetailViewModel
    {
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Telefon Numarası")]
        public string PhoneNumber { get; set; }
        public static implicit operator AppUser(UserDetailViewModel userDetail)
        {
            return new AppUser
            {
                UserName = userDetail.UserName,
                Email = userDetail.Email,
                PhoneNumber = userDetail.PhoneNumber
            };
        }
    }
}
