using Arch.EntityLayer.Entities;

namespace Arch.UI.Models
{
    public class CreateUserVM
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public static implicit operator AppUser(CreateUserVM createUserVM)
        {
            return new AppUser
            {
                UserName = createUserVM.Username,
                Email = createUserVM.Email
            };
        }
    }
}
