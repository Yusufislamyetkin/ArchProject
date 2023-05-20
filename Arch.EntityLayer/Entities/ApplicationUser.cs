using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arch.EntityLayer.Entities
{
    public class ApplicationUser : IdentityUser
    {
        // Özel kullanıcı alanları
        public virtual Customer Customer { get; set; }
        public virtual Designer Designer { get; set; }
    }
}
