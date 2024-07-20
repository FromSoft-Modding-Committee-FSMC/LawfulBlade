namespace LawfulBladeManager.Type
{
    public static class UriExtensions
    {
        /// <summary>
        /// Adds text to the end of a URI
        /// </summary>
        public static Uri Append(this Uri uri, string text) =>
            new($"{uri}{text}");

        public static Uri Combine(this Uri uri, params string[] parts)
        {
            // Use a string as a buffer...
            string buffer = $"{uri}";

            // Try to append each element...
            foreach(string part in parts)
                buffer = $"{buffer.TrimEnd('\\', '/')}/{part.TrimStart('\\', '/')}";

            return new Uri(buffer);
        }
    }
}
