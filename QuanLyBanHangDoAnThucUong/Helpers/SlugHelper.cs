using System.Text;
using System.Text.RegularExpressions;

namespace QuanLyBanHangDoAnThucUong.Helpers
{
    public static class SlugHelper
    {
        public static string GenerateSlug(string phrase)
        {
            if (string.IsNullOrEmpty(phrase)) return string.Empty;

            string str = RemoveAccent(phrase).ToLower();
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            str = Regex.Replace(str, @"\s+", " ").Trim();
            str = Regex.Replace(str, @"\s", "-");
            str = Regex.Replace(str, @"-+", "-");
            return str;
        }

        private static string RemoveAccent(string txt)
        {
            return ConvertToUnSign(txt);
        }

        public static string ConvertToUnSign(string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;

            var regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, string.Empty)
                        .Replace('\u0111', 'd')
                        .Replace('\u0110', 'D');
        }

        public static string GenerateSlugOptimal(string phrase)
        {
            if (string.IsNullOrWhiteSpace(phrase)) return "";
            var str = ConvertToUnSign(phrase).ToLowerInvariant();
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            str = Regex.Replace(str, @"\s+", "-").Trim();
            str = Regex.Replace(str, @"-+", "-");
            return str;
        }
    }
}
