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

        /// <summary>
        /// Returns the last element of an array of strings
        /// </summary>
        public static string Back(this string[] stringArr) =>
            stringArr[stringArr.Length - 1];

        /// <summary>
        /// Appends an item to the string array
        /// </summary>
        /// <param name="stringArr"></param>
        /// <param name="merge"></param>
        /// <returns></returns>
        public static string[] Merge(this string[] stringArr, string merge)
        {
            string[] newArray = new string[stringArr.Length + 1];

            Array.Copy(stringArr, newArray, stringArr.Length);

            newArray[^1] = merge;

            return newArray;
        }

        public static string[] Remove(this string[] stringArr, int index)
        {
            string[] newArray = new string[stringArr.Length - 1];

            int writePos = 0;
            for (int readPos = 0; readPos < stringArr.Length; ++readPos)
            {
                if (readPos == index)
                    continue;

                newArray[writePos++] = stringArr[readPos];
            }

            return newArray;
        }

        public static int IndexOf(this string[] stringArr, string value)
        {
            for (int i = 0; i < stringArr.Length; ++i)
            {
                if (stringArr[i] == value)
                    return i;
            }

            return -1;
        }
    }
}
