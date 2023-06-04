using Microsoft.AspNetCore.Identity;

namespace Arch.EntityLayer.Entities.Auth
{
    public class Authorization
    {
        public class AppUser : IdentityUser<string>
        {
            public string ProfilPhoto { get; set; }
            public string City { get; set; }
            public ICollection<IdentityUserRole<string>> UserRoles { get; set; } = new List<IdentityUserRole<string>>();
        }

        public class AppRole : IdentityRole<string>
        {

            public const string Customer = "Customer";
            public const string Designer = "Designer";
            public const string Admin = "Admin";
        }
    }
}

