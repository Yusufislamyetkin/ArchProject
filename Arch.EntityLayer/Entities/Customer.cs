﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arch.EntityLayer.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public ICollection<Competition> Competitions { get; set; } // Yarışmalara referans için Competitions koleksiyonu
    }
}
