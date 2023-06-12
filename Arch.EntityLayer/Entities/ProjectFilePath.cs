using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arch.EntityLayer.Entities
{
    public class ProjectFilePath
    {
        public int Id { get; set; }

        public int CompetitionId { get; set; }
        public Competition Competition { get; set; }


        public string Address { get; set; }
    }
}
