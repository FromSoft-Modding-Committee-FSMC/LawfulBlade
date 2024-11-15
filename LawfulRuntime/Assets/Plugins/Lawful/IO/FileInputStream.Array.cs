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

        public sbyte[] ReadS8Array(int count)
        {
            sbyte[] s8s = new sbyte[count];

            for (int i = 0; i < count; ++i)
                s8s[i] = ReadS8();

            return s8s;
        }

        /// <summary>
        /// Reads an array of U16 values
        /// </summary>
        /// <param name="count">The number of u16 values to read</param>
        /// <param name="endianess">Endianness to read the u16s as</param>
        public ushort[] ReadU16Array(int count, Endianness endianess = Endianness.Little)
        {
            ushort[] u16s = new ushort[count];

            for (int i = 0; i < count; ++i)
                u16s[i] = ReadU16(endianess);

            return u16s;
        }
    }
}

