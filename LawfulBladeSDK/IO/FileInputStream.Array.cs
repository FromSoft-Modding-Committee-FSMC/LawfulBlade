namespace LawfulBladeSDK.IO
{
    public partial class FileInputStream
    {
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
        /// Reads an array of signed 32-bit values
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

        /// <summary>
        /// Reads an array of signed 64-bit values
        /// </summary>
        /// <param name="count">The number of values to read</param>
        /// <param name="endianness">Endianness to read as</param>
        public long[] ReadS64Array(int count, Endianness endianness = Endianness.Little)
        {
            long[] s64s = new long[count];

            for (int i = 0; i < count; ++i)
                s64s[i] = ReadS64(endianness);

            return s64s;
        }

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
        /// Reads an array of unsigned 64-bit values
        /// </summary>
        /// <param name="count">The number of values to read</param>
        /// <param name="endianness">Endianness to read as</param>
        public ulong[] ReadU64Array(int count, Endianness endianness = Endianness.Little)
        {
            ulong[] u64s = new ulong[count];

            for (int i = 0; i < count; ++i)
                u64s[i] = ReadU64(endianness);

            return u64s;
        }

        /// <summary>
        /// Reads an array of 32-bit float values
        /// </summary>
        /// <param name="count">The number of values to read</param>
        /// <param name="endianness">Endianness to read as</param>
        public float[] ReadF32Array(int count, Endianness endianness = Endianness.Little)
        {
            float[] f32s = new float[count];

            for (int i = 0; i < count; ++i)
                f32s[i] = ReadF32(endianness);

            return f32s;
        }

        /// <summary>
        /// Reads an array of 64-bit float values
        /// </summary>
        /// <param name="count">The number of values to read</param>
        /// <param name="endianness">Endianness to read as</param>
        public double[] ReadF64Array(int count, Endianness endianness = Endianness.Little)
        {
            double[] f64s = new double[count];

            for (int i = 0; i < count; ++i)
                f64s[i] = ReadF64(endianness);

            return f64s;
        }
    }
}

