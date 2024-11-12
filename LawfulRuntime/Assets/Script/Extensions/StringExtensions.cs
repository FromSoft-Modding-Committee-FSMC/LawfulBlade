using UnityEngine;

public static class StringExtensions
{
    /// <summary>
    /// Sanitizes a fixed length string, removing everything after (and including) the first null character.
    /// </summary>
    /// <param name="s">The string to sanitize</param>
    /// <returns>The sanitized string</returns>
    public static string Sanitize(this string s) =>
        s[..s.IndexOf('\0')];
        
}
