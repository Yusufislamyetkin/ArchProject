using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arch.EntityLayer.Entities.Auth
{
    public class Authorization
    {
        public class AppUser : IdentityUser<string>
        {
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

