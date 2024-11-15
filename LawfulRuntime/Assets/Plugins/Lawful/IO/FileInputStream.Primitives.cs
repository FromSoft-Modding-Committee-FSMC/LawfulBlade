using System;

namespace Lawful.IO
{
    public partial class FileInputStream
    {
        /// <summary>
        /// Read a signed byte (8 bit number) from the stream
        /// </summary>
        public sbyte ReadS8()
        {
            fstream.Read(buffer, 0, 1);
            return (sbyte)buffer[0];
        }

        /// <summary>
        /// Read a signed short (16 bit number) from the stream
        /// </summary>
        public short ReadS16(Endianness endianness = Endianness.Little)
        {
            fstream.Read(buffer, 0, 2);

            if ((endianness == Endianness.Big && BitConverter.IsLittleEndian) || (endianness == Endianness.Little && !BitConverter.IsLittleEndian))
                return (short)(buffer[0] << 8 | buffer[1]);

            return (short)(buffer[1] << 8 | buffer[0]);
        }

        /// <summary>
        /// Read a signed int (32 bit number) from the stream
        /// </summary>
        public int ReadS32(Endianness endianness = Endianness.Little)
        {
            fstream.Read(buffer, 0, 4);

            if ((endianness == Endianness.Big && BitConverter.IsLittleEndian) || (endianness == Endianness.Little && !BitConverter.IsLittleEndian))
                return buffer[0] << 24 | buffer[1] << 16 | buffer[2] << 8 | buffer[3];

            return buffer[3] << 24 | buffer[2] << 16 | buffer[1] << 8 | buffer[0];
        }
        
        /// <summary>
        /// Read a signed long (64 bit number) from the stream
        /// </summary>
        public long ReadS64(Endianness endianness = Endianness.Little)
        {
            fstream.Read(buffer, 0, 8);

            if ((endianness == Endianness.Big && BitConverter.IsLittleEndian) || (endianness == Endianness.Little && !BitConverter.IsLittleEndian))
                return buffer[0] << 56 | buffer[1] << 48 | buffer[2] << 40 | buffer[3] << 32 | buffer[4] << 24 | buffer[5] << 16 | buffer[6] << 8 | buffer[7];

            return buffer[7] << 56 | buffer[6] << 48 | buffer[5] << 40 | buffer[4] << 32 | buffer[3] << 24 | buffer[2] << 16 | buffer[1] << 8 | buffer[0];
        }

        /// <summary>
        /// Read a unsigned byte (8 bit number) from the stream
        /// </summary>
        public byte ReadU8()
        {
            fstream.Read(buffer, 0, 1);
            return buffer[0];
        }

        /// <summary>
        /// Read a unsigned short (16 bit number) from the stream
        /// </summary>
        public ushort ReadU16(Endianness endianness = Endianness.Little)
        {
            fstream.Read(buffer, 0, 2);

            if ((endianness == Endianness.Big && BitConverter.IsLittleEndian) || (endianness == Endianness.Little && !BitConverter.IsLittleEndian))
                return (ushort)(buffer[0] << 8 | buffer[1]);

            return (ushort)(buffer[1] << 8 | buffer[0]);
        }

        /// <summary>
        /// Read a unsigned int (32 bit number) from the stream
        /// </summary>
        public uint ReadU32(Endianness endianness = Endianness.Little)
        {
            fstream.Read(buffer, 0, 4);

            if ((endianness == Endianness.Big && BitConverter.IsLittleEndian) || (endianness == Endianness.Little && !BitConverter.IsLittleEndian))
                return (uint)(buffer[0] << 24 | buffer[1] << 16 | buffer[2] << 8 | buffer[3]);

            return (uint)(buffer[3] << 24 | buffer[2] << 16 | buffer[1] << 8 | buffer[0]);
        }

        /// <summary>
        /// Read a unsigned long (64 bit number) from the stream
        /// </summary>
        public ulong ReadU64(Endianness endianness = Endianness.Little)
        {
            fstream.Read(buffer, 0, 8);

            if ((endianness == Endianness.Big && BitConverter.IsLittleEndian) || (endianness == Endianness.Little && !BitConverter.IsLittleEndian))
                return (ulong)(buffer[0] << 56 | buffer[1] << 48 | buffer[2] << 40 | buffer[3] << 32 | buffer[4] << 24 | buffer[5] << 16 | buffer[6] << 8 | buffer[7]);

            return (ulong)(buffer[7] << 56 | buffer[6] << 48 | buffer[5] << 40 | buffer[4] << 32 | buffer[3] << 24 | buffer[2] << 16 | buffer[1] << 8 | buffer[0]);
        }

        /// <summary>
        /// Read a floating point single (32 bit decimal number) from the stream
        /// </summary>
        public float ReadF32(Endianness endianness = Endianness.Little)
        {
            fstream.Read(buffer, 0, 4);

            if ((endianness == Endianness.Big && BitConverter.IsLittleEndian) || (endianness == Endianness.Little && !BitConverter.IsLittleEndian))
            {
                //Shuffle buffered bytes to swap endianness
                buffer[4] = buffer[0];  //Swap Bytes 0 & 3
                buffer[0] = buffer[3];
                buffer[3] = buffer[4];
                buffer[4] = buffer[1];  //Swap Bytes 1 & 2
                buffer[1] = buffer[2];
                buffer[2] = buffer[4];
            }

            return BitConverter.ToSingle(buffer, 0);
        }

        /// <summary>
        /// Read a floating point double (64 bit decimal number) from the stream
        /// </summary>
        public double ReadF64(Endianness endianness = Endianness.Little)
        {
            fstream.Read(buffer, 0, 8);

            if ((endianness == Endianness.Big && BitConverter.IsLittleEndian) || (endianness == Endianness.Little && !BitConverter.IsLittleEndian))
            {
                //Shuffle buffered bytes to swap endianness
                buffer[8] = buffer[0];  //Swap bytes 0 & 7
                buffer[0] = buffer[7];
                buffer[7] = buffer[8];
                buffer[8] = buffer[1];  //Swap bytes 1 & 6
                buffer[1] = buffer[6];
                buffer[6] = buffer[8];
                buffer[8] = buffer[2];  //Swap bytes 2 & 5
                buffer[2] = buffer[5];
                buffer[5] = buffer[8];
                buffer[8] = buffer[3];  //Swap bytes 3 & 4
                buffer[3] = buffer[4];
                buffer[4] = buffer[8];
            }

            return BitConverter.ToDouble(buffer, 0);
        }
    }
}