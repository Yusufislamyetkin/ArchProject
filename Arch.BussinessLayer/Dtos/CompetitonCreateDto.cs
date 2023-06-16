using Microsoft.AspNetCore.Http;
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
        [MinimumWordCount(50, ErrorMessage = "Proje Açıklaması en az 50 kelime olmalıdır.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Lütfen Proje Fiyatını boş geçmeyiniz...")]
        public int Price { get; set; }
        [Required(ErrorMessage = "Lütfen Proje Bitiş Tarihini boş geçmeyiniz...")]
        public DateTime EndDate { get; set; }

        [RequiredFile(ErrorMessage = "Dosya yüklenmedi. Lütfen dosya seçiniz.")]
        public IFormFile[] files { get; set; }

        public string? CustomerId { get; set; } // Customer referansı için CustomerId
    }

    public class MinimumWordCountAttribute : ValidationAttribute
    {
        private readonly int _minimumWordCount;

        public MinimumWordCountAttribute(int minimumWordCount)
        {
            _minimumWordCount = minimumWordCount;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var description = value.ToString();
                var words = description.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (words.Length < _minimumWordCount)
                {
                    return new ValidationResult($"Proje Açıklaması en az {_minimumWordCount} kelime olmalıdır.");
                }
            }

            return ValidationResult.Success;
        }
    }

    public class RequiredFileAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is IFormFile[] files)
            {
                foreach (var file in files)
                {
                    if (file != null && file.Length > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
