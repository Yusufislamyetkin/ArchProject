using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Arch.EntityLayer.Entities.Auth.Authorization;

namespace Arch.EntityLayer.Entities
{
    public class Competition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProjectType { get; set; }
        public int ProjectShape { get; set; }
        public int Field { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string Destinition { get; set; }
        public string Insteresting { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }

        public string CustomerId { get; set; } // Customer referansı için CustomerId
        public AppUser Customer { get; set; } // Customer referansı

        public ICollection<AppUser> Designers { get; set; } // Designer'ların referansı için Designers koleksiyonu

        public ICollection<File> Files { get; set; } // Dosyalara referans için Files koleksiyonu

        public Competition()
        {
            Designers = new List<AppUser>();
        }
    }

}
