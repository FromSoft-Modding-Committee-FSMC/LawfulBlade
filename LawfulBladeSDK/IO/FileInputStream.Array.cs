namespace LawfulBladeSDK.IO
{
    public partial class FileInputStream
    {
        /// <summary>
        /// Reads an array of unsigned 8-bit values
        /// </summary>
        /// <param name="count">The number of values to read</param>
        public byte[] ReadU8Array(int count)
        {
            byte[] u8s = new byte[count];

            for (int i = 0; i < count; ++i)
                u8s[i] = ReadU8();

            return u8s;
        }

        /// <summary>
        /// Reads an array of unsigned 16-bit values
        /// </summary>
        /// <param name="count">The number of values to read</param>
        /// <param name="endianness">Endianness to read as</param>
        public ushort[] ReadU16Array(int count, Endianness endianness = Endianness.Little)
        {
            ushort[] u16s = new ushort[count];

            for (int i = 0; i < count; ++i)
                u16s[i] = ReadU16(endianness);

            return u16s;
        }

        /// <summary>
        /// Reads an array of unsigned 32-bit values
        /// </summary>
        /// <param name="count">The number of values to read</param>
        /// <param name="endianness">Endianness to read as</param>
        public uint[] ReadU32Array(int count, Endianness endianness = Endianness.Little)
        {
            uint[] u32s = new uint[count];

            for (int i = 0; i < count; ++i)
                u32s[i] = ReadU32(endianness);

            return u32s;
        }

        /// <summary>
        /// Reads an array of signed 8-bit values
        /// </summary>
        /// <param name="count">The number of values to read</param>
        public sbyte[] ReadS8Array(int count)
        {
            sbyte[] s8s = new sbyte[count];

            for (int i = 0; i < count; ++i)
                s8s[i] = ReadS8();

            return s8s;
        }

        /// <summary>
        /// Reads an array of signed 16-bit values
        /// </summary>
        /// <param name="count">The number of values to read</param>
        /// <param name="endianness">Endianness to read as</param>
        public short[] ReadS16Array(int count, Endianness endianness = Endianness.Little)
        {
            short[] s16s = new short[count];

            for (int i = 0; i < count; ++i)
                s16s[i] = ReadS16(endianness);

            return s16s;
        }

        /// <summary>
        /// Reads an array of unsigned 32-bit values
        /// </summary>
        /// <param name="count">The number of values to read</param>
        /// <param name="endianness">Endianness to read as</param>
        public int[] ReadS32Array(int count, Endianness endianness = Endianness.Little)
        {
            int[] s32s = new int[count];

            for (int i = 0; i < count; ++i)
                s32s[i] = ReadS32(endianness);

            return s32s;
        }
    }
}

