using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arch.BussinessLayer.Dtos
{
    public class CompetitonCreateDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Lütfen Proje adını boş geçmeyiniz...")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Lütfen Proje Tipini boş geçmeyiniz...")]
        public int ProjectType { get; set; }
        [Required(ErrorMessage = "Lütfen Proje Alanını boş geçmeyiniz...")]
        public int Field { get; set; }
        [Required(ErrorMessage = "Lütfen Proje Açıklamasını boş geçmeyiniz...")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Lütfen Proje Fiyatını boş geçmeyiniz...")]
        public int Price { get; set; }
        [Required(ErrorMessage = "Lütfen Proje Bitiş Tarihini boş geçmeyiniz...")]
        public DateTime EndDate { get; set; }
        public string? CustomerId { get; set; } // Customer referansı için CustomerId
    }
}
