using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arch.BussinessLayer.Dtos
{
    public class SelectionViewModel
    {
        public string DesignerId { get; set; }
        public string DesignerName { get; set; }
        public int SelectedOption { get; set; }
        public int CompetitionId { get; set; }
    }
}
