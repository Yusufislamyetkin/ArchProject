using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arch.BussinessLayer.Dtos
{
    public class CommentViewModel
    {
        public int PostId { get; set; }

        [Required(ErrorMessage = "Adınızı giriniz.")]
        public string CommenterName { get; set; }

        [Required(ErrorMessage = "Yorumunuzu giriniz.")]
        public string CommentText { get; set; }
    }
}
