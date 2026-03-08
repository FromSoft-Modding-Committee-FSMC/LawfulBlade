namespace LawfulBladeSDK.IO
{
    public partial class FileOutputStream
    {
        /// <summary>
        /// Write a signed byte (8 bit number) to the stream
        /// </summary>
        public void WriteS8(sbyte value)
        {
            fstream.WriteByte(unchecked((byte)value));
        }

        /// <summary>
        /// Write a signed short (16 bit number) to the stream
        /// </summary>
        public void WriteS16(short value, Endianness endianness = Endianness.Little)
        {
            Span<byte> buffer = stackalloc byte[2];
            BitConverter.TryWriteBytes(buffer, value);

            if ((endianness == Endianness.Big && BitConverter.IsLittleEndian) || (endianness == Endianness.Little && !BitConverter.IsLittleEndian))
                buffer.Reverse();

            fstream.Write(buffer);
        }

        /// <summary>
        /// Write a signed int (32 bit number) to the stream
        /// </summary>
        public void WriteS32(int value, Endianness endianness = Endianness.Little)
        {
            Span<byte> buffer = stackalloc byte[4];
            BitConverter.TryWriteBytes(buffer, value);

            if ((endianness == Endianness.Big && BitConverter.IsLittleEndian) || (endianness == Endianness.Little && !BitConverter.IsLittleEndian))
                buffer.Reverse();

            fstream.Write(buffer);
        }

        /// <summary>
        /// Write a signed int (64 bit number) to the stream
        /// </summary>
        public void WriteS64(long value, Endianness endianness = Endianness.Little)
        {
            Span<byte> buffer = stackalloc byte[8];
            BitConverter.TryWriteBytes(buffer, value);

            if ((endianness == Endianness.Big && BitConverter.IsLittleEndian) || (endianness == Endianness.Little && !BitConverter.IsLittleEndian))
                buffer.Reverse();

            fstream.Write(buffer);
        }


        /// <summary>
        /// Write a unsigned byte (8 bit number) to the stream
        /// </summary>
        public void WriteU8(byte value)
        {
            fstream.WriteByte(value);
        }

        /// <summary>
        /// Write a unsigned short (16 bit number) to the stream
        /// </summary>
        public void WriteU16(ushort value, Endianness endianness = Endianness.Little)
        {
            Span<byte> buffer = stackalloc byte[2];
            BitConverter.TryWriteBytes(buffer, value);

            if ((endianness == Endianness.Big && BitConverter.IsLittleEndian) || (endianness == Endianness.Little && !BitConverter.IsLittleEndian))
                buffer.Reverse();

            fstream.Write(buffer);
        }

        /// <summary>
        /// Write a unsigned int (32 bit number) to the stream
        /// </summary>
        public void WriteU32(uint value, Endianness endianness = Endianness.Little)
        {
            Span<byte> buffer = stackalloc byte[4];
            BitConverter.TryWriteBytes(buffer, value);

            if ((endianness == Endianness.Big && BitConverter.IsLittleEndian) || (endianness == Endianness.Little && !BitConverter.IsLittleEndian))
                buffer.Reverse();

            fstream.Write(buffer);
        }

        /// <summary>
        /// Write a unsigned long (64 bit number) to the stream
        /// </summary>
        public void WriteU64(ulong value, Endianness endianness = Endianness.Little)
        {
            Span<byte> buffer = stackalloc byte[8];
            BitConverter.TryWriteBytes(buffer, value);

            if ((endianness == Endianness.Big && BitConverter.IsLittleEndian) || (endianness == Endianness.Little && !BitConverter.IsLittleEndian))
                buffer.Reverse();

            fstream.Write(buffer);
        }


        /// <summary>
        /// Write a floating point half (16 bit decimal number) to the stream
        /// </summary>
        public void WriteF16(Half value, Endianness endianness = Endianness.Little)
        {
            Span<byte> buffer = stackalloc byte[2];
            BitConverter.TryWriteBytes(buffer, value);

            if ((endianness == Endianness.Big && BitConverter.IsLittleEndian) || (endianness == Endianness.Little && !BitConverter.IsLittleEndian))
                buffer.Reverse();

            fstream.Write(buffer);
        }

        /// <summary>
        /// Write a floating point single (32 bit decimal number) to the stream
        /// </summary>
        public void WriteF32(float value, Endianness endianness = Endianness.Little)
        {
            Span<byte> buffer = stackalloc byte[4];
            BitConverter.TryWriteBytes(buffer, value);

            if ((endianness == Endianness.Big && BitConverter.IsLittleEndian) || (endianness == Endianness.Little && !BitConverter.IsLittleEndian))
                buffer.Reverse();

            fstream.Write(buffer);
        }

        /// <summary>
        /// Write a floating point double (64 bit decimal number) to the stream
        /// </summary>
        public void WriteF64(double value, Endianness endianness = Endianness.Little)
        {
            Span<byte> buffer = stackalloc byte[8];
            BitConverter.TryWriteBytes(buffer, value);

            if ((endianness == Endianness.Big && BitConverter.IsLittleEndian) || (endianness == Endianness.Little && !BitConverter.IsLittleEndian))
                buffer.Reverse();

            fstream.Write(buffer);
        }
    }
}