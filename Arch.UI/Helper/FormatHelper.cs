using System.Globalization;

namespace Arch.UI.Helper
{
    public static class FormatHelper
    {
        public static string FormatAmount(decimal amount)
        {
            return amount.ToString("N", new CultureInfo("tr-TR")); // 'tr-TR' Türkçe formatında para birimi için
        }

        // Dosya adını alma işlemi
        public static string GetFileName(string filePath)
        {
            return Path.GetFileName(filePath);
        }

        // Dosya türünü alma işlemi
        public static string GetFileType(string filePath)
        {
            string extension = Path.GetExtension(filePath);

            switch (extension.ToLower())
            {
                case ".png":
                case ".jpg":
                case ".jpeg":
                    return "image";
                case ".doc":
                case ".docx":
                    return "word";
                case ".pdf":
                    return "pdf";
                case ".msg":
                    return "email";
                default:
                    return "other";
            }
        }
    }
}
