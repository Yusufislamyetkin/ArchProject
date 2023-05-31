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
        public string Name { get; set; } // Proje Adı
        public int ProjectType { get; set; } // Tadilat , İç mimari , Peyzaj, Yeni Konut
        public int Field { get; set; } // Metre Kare
        public string Description { get; set; } // Proje ile ilgili Açıklama Giriniz
        public int Price { get; set; } // Yaptırmak istediğiniz Fiyat (Minimum Tutardan Aşşağı olamaz MetreKare ile hesaplanacak)
        public DateTime EndDate { get; set; } // Bitiş Tarihi
        public int Status { get; set; } // Ödeme'si yapıldı mı admin onayı aldı mı.

        public string CustomerId { get; set; } // Customer referansı için CustomerId
        public AppUser Customer { get; set; } // Customer referansı

        public string? DesignerId { get; set; } // Customer referansı için CustomerId
        public ICollection<AppUser> Designers { get; set; } // Designer'ların referansı için Designers koleksiyonu

        public string? FileId { get; set; } // Customer referansı için CustomerId
        public ICollection<File> Files { get; set; } // Dosyalara referans için Files koleksiyonu

        public ICollection<BlogPost> BlogPosts { get; set; }


    }

}
