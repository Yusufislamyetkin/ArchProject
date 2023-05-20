using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arch.EntityLayer.Entities
{
    public class File
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }

        public int CompetitionsId { get; set; } // Yarışmaya referans için CompetitionsId özelliği
        public Competition Competitions { get; set; } // Yarışmaya referans için Competitions özelliği
    }
}
