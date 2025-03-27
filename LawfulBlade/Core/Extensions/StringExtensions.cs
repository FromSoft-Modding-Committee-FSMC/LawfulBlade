namespace LawfulBlade.Core.Extensions
{
    internal static class StringExtensions
    {
        /// <summary>
        /// Removes all occurences of a given character from a string
        /// </summary>
        public static string Strip(this string str, char charToStrip) =>
            str.Replace(charToStrip.ToString(), string.Empty);

        /// <summary>
        /// Returns only valid numerics (0 - 9, .)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetDigits(this string str) =>
            new([.. str.Where(c => (char.IsDigit(c) || c == '.'))]);

    }
}
