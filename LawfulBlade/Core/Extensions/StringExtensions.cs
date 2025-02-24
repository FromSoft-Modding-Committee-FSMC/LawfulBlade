namespace LawfulBlade.Core.Extensions
{
    internal static class StringExtensions
    {
        /// <summary>
        /// Removes all occurences of a given character from a string
        /// </summary>
        public static string Strip(this string str, char charToStrip) =>
            str.Replace(charToStrip.ToString(), string.Empty);
    }
}
