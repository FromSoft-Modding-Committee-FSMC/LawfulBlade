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
    }
}
