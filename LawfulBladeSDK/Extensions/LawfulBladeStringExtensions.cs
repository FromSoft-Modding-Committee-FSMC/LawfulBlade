using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawfulBladeSDK.Extensions
{
    /// <summary>
    /// Extensions for the string data type.
    /// </summary>
    public static class LawfulBladeStringExtensions
    {
        /// <summary>
        /// Wraps a string with ansi colour codes
        /// </summary>
        public static string Colourise(this string text, uint colour) =>
            $"\u001b[38;2;{(colour >> 16) & 0xFF};{(colour >> 8) & 0xFF};{(colour >> 0) & 0xFF}m{text}\u001b[0m";
    }
}
