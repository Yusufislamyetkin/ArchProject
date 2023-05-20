using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public int CustomerId { get; set; } // Müşteriye referans için CustomerId özelliği
        public Customer Customer { get; set; } // Müşteriye referans için Customer özelliği

        public ICollection<File> Files { get; set; } // Dosyalara referans için Files koleksiyonu
        public ICollection<Designer> Designers { get; set; } // Tasarımcılara referans için Designers koleksiyonu
    }
}
