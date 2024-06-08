using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Zuxi.OSC.utility
{
   public static class StringExtensions
    {
        public static string RemoveNonUtf8Chars(this string input)
        {
            // Define the pattern to match UTF-8 characters
            string pattern = @"[^\u0000-\u007F]"; // Matches any character outside the ASCII range (non-UTF-8)

            // Replace non-UTF-8 characters with an empty string
            return Regex.Replace(input, pattern, "");
        }
    }
}
