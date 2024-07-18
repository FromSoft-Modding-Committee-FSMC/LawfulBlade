namespace LawfulBladeManager.Type
{
    public static class StringExtensions
    {
        public static bool IsAlphanumeric(this string str)
        {
            bool isAlphanumeric = true;
            foreach (char c in str)
                isAlphanumeric &= Char.IsAsciiLetterOrDigit(c);
            return isAlphanumeric;
        }

        /// <summary>
        /// Removes any occurances of a character literal.
        /// </summary>
        /// <param name="charToRemove">The char to remove</param>
        /// <returns>original string but with the char removed</returns>
        public static string Remove(this string str, char c) =>
            str.Replace(c.ToString(), string.Empty);

    }
}
