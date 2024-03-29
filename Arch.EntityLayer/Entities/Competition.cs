﻿using Arch.EntityLayer.Entities.Auth;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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
        public string Goal { get; set; } // Amacı
        public string Priorities { get; set; } // Proje Öncelikleri
        public string InspirationAddress1{ get; set; } // Proje İlham Linki1
        public string InspirationAddress2 { get; set; } //  Proje İlham Linki2
        public int Price { get; set; } // Yaptırmak istediğiniz Fiyat (Minimum Tutardan Aşşağı olamaz MetreKare ile hesaplanacak)
        public DateTime EndDate { get; set; } // Bitiş Tarihi
        public int Status { get; set; } // Ödeme'si yapıldı mı admin onayı aldı mı.

        public string CustomerId { get; set; } // Customer referansı için CustomerId
        public AppUser Customer { get; set; } // Customer referansı

        public int? DesignerId { get; set; } // Customer referansı için CustomerId
        [JsonIgnore]
        public ICollection<DesignerUser> DesignerUsers { get; set; } // Customer referansı

        public int? BlogPostId { get; set; }
        public ICollection<BlogPost> BlogPosts { get; set; }

        public int? ProjectFilePathID { get; set; }
        public ICollection<ProjectFilePath>   ProjectFilePaths { get; set; }


        public int? RewardId { get; set; } // Customer referansı için CustomerId
        [JsonIgnore]
        public ICollection<Reward>  Rewards { get; set; } // Customer referansı




    }

}
