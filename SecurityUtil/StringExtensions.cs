using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityUtil
{
    public static class StringExtensions
    {
        public static string EncodeToBase64String(this string source)
        {
            string s = source.Trim().Replace(" ", "+");
            s = (s.Length % 4 > 0) ? s.PadRight(s.Length + 4 - s.Length % 4, '=') : s;
            return Encoding.UTF8.GetString(Convert.FromBase64String(s));
        }
    }
}