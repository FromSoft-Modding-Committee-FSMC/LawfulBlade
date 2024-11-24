using System.Text;
using UnityEngine;

namespace Lawful.IO
{
    public partial class FileInputStream
    {
        public string ReadTerminatedString() =>
            ReadTerminatedString(Encoding.ASCII);

        public string ReadTerminatedString(Encoding encoding)
        {
            byte[] charBuffer = new byte[256];
            int charCounter = 0;

            do
            {
                charBuffer[charCounter] = ReadU8();
            } while (charBuffer[charCounter++] != 0x00);

            string terminatedString = encoding.GetString(charBuffer, 0, charCounter);

            return terminatedString[..terminatedString.IndexOf('\0')];
        }

        /// <summary>
        /// Reads a fixed length ascii string. Everything after a null terminator (\0) is removed.
        /// </summary>
        /// <param name="length">The length of the string to read</param>
        /// <returns>duh? The string dumbass</returns>
        public string ReadFixedString(int length) =>
            ReadFixedString(length, Encoding.ASCII);

        /// <summary>
        /// Reads a fixed length string in a specific encoding. Everything after a null terminator (\0) is removed.
        /// </summary>
        /// <param name="length">The length of the string to read</param>
        /// <param name="encoding">The encoding of the string</param>
        /// <returns>The string</returns>
        public string ReadFixedString(int length, Encoding encoding)
        {
            fstream.Read(buffer, 0, length);

            string decodedString = encoding.GetString(buffer, 0, length);

            return decodedString[..decodedString.IndexOf('\0')];
        }
    }
}
