using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arch.BussinessLayer.Dtos
{
    public class CompetitonCreateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProjectType { get; set; }
        public int Field { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public DateTime EndDate { get; set; }
        public string? CustomerId { get; set; } // Customer referansı için CustomerId
    }
}
