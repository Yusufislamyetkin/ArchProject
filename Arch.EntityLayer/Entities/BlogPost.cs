using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Arch.EntityLayer.Entities.Auth.Authorization;

namespace Arch.EntityLayer.Entities
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string AuthorId { get; set; }
        public AppUser Author { get; set; }
        public DateTime CreatedDate { get; set; }

        public int CompetitionId { get; set; }
        public Competition Competition { get; set; }

        [NotMapped]
        public string TimeAgo
        {
            get { return Arch.EntityLayer.StaticClass.TimeAgo.GetTimeAgo(CreatedDate); }
        }



    }

}
