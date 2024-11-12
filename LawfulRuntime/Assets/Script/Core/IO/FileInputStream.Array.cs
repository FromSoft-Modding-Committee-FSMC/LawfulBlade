using UnityEngine;

namespace Lawful.IO
{
    public partial class FileInputStream
    {
        /// <summary>
        /// Reads an array of U8 values
        /// </summary>
        /// <param name="count">The number of U8 values to read</param>
        public byte[] ReadU8Array(int count)
        {
            byte[] bytes = new byte[count];
            fstream.Read(bytes, 0, count);

            return bytes;
        }
    }
}

