using Arch.EntityLayer.Entities;

namespace Arch.UI.Models
{
    public class CompetitionViewModel
    {
        public List<Competition> ActiveCompetitions { get; set; }
        public List<Competition> FinishedCompetitions { get; set; }
    }
}
