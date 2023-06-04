using System.Globalization;

namespace Arch.UI.Helper
{
    public static class FormatHelper
    {
        public static string FormatAmount(decimal amount)
        {
            return amount.ToString("N", new CultureInfo("tr-TR")); // 'tr-TR' Türkçe formatında para birimi için
        }
    }
}
