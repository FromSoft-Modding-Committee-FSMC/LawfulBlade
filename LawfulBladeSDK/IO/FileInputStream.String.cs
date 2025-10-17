using System.Text;

namespace LawfulBladeSDK.IO
{
    public partial class FileInputStream
    {
        /// <summary>
        /// Reads a null terminated (c style) ascii string.
        /// </summary>
        public string ReadTerminatedString() =>
            ReadTerminatedString(Encoding.ASCII);

        /// <summary>
        /// Reads a null terminated (c style) string in a specific encoding.
        /// </summary>
        /// <param name="encoding">The encoding of the string</param>
        public string ReadTerminatedString(Encoding encoding)
        {
            int c = 0;

            do
            {
                buffer[c] = ReadU8(); 

                // Bounds check. If a string is longer than the max buffer size, something is very wrong.
                if (c >= BYTEBUFFER_SIZE)
                    throw new Exception($"Tried to read a null-terminated string longer than {BYTEBUFFER_SIZE} characters.");

            } while (buffer[c++] != 0x00);

            string terminatedString = encoding.GetString(buffer, 0, c - 1);

            // We don't want to include the null terminator in the C# string, so find the index of it - and then only return what is before.
            int nullTerminatorPosition = terminatedString.IndexOf('\0');

            if (nullTerminatorPosition == -1)
                throw new Exception("Tried to read a null terminated string which does not contain a null terminator.");

            return terminatedString[..nullTerminatorPosition];
        }

        /// <summary>
        /// Reads a fixed length ascii string. Everything after a null terminator (\0) is removed.
        /// </summary>
        /// <param name="length">The length of the string to read</param>
        public string ReadFixedString(int length) =>
            ReadFixedString(length, Encoding.ASCII);

        /// <summary>
        /// Reads a fixed length string in a specific encoding. Everything after a null terminator (\0) is removed.
        /// </summary>
        /// <param name="length">The length of the string to read</param>
        /// <param name="encoding">The encoding of the string</param>
        public string ReadFixedString(int length, Encoding encoding)
        {
            fstream.ReadExactly(buffer, 0, length);

            string decodedString = encoding.GetString(buffer, 0, length);

            return decodedString[..decodedString.IndexOf('\0')];
        }
    }
}
