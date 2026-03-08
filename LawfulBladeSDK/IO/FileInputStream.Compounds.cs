using System.Runtime.InteropServices;

namespace LawfulBladeSDK.IO
{
    public partial class FileInputStream
    {
        /// <summary>
        /// Reads a structure of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The structure type to read.<br/>
        /// You should make sure alignment and packing matches that of the data inside the stream using 
        /// the <b>StructLayout</b> attribute.</typeparam>
        public T ReadStruct<T>() where T : unmanaged
        {
            // Get the struct size
            int structSize = Marshal.SizeOf<T>();

            // Read the required number of bytes to create the struct
            Span<byte> buffer = stackalloc byte[structSize];
            fstream.ReadExactly(buffer);

            // Unsafe win
            return MemoryMarshal.Read<T>(buffer);
        }

        public T ReadEnum<T>(Endianness endianness = Endianness.Little) where T : Enum
        {
            Type underlyingType = Enum.GetUnderlyingType(typeof(T));
            int enumSize        = Marshal.SizeOf(underlyingType);

            Span<byte> buffer = stackalloc byte[enumSize];
            fstream.ReadExactly(buffer);

            if ((endianness == Endianness.Big && BitConverter.IsLittleEndian) || (endianness == Endianness.Little && !BitConverter.IsLittleEndian))
                buffer.Reverse();

            object value = underlyingType switch
            {
                Type t when t == typeof(byte)   => buffer[0],
                Type t when t == typeof(sbyte)  => unchecked((sbyte)buffer[0]),
                Type t when t == typeof(short)  => BitConverter.ToInt16(buffer),
                Type t when t == typeof(ushort) => BitConverter.ToUInt16(buffer),
                Type t when t == typeof(int)    => BitConverter.ToInt32(buffer),
                Type t when t == typeof(uint)   => BitConverter.ToUInt32(buffer),
                Type t when t == typeof(long)   => BitConverter.ToInt64(buffer),
                Type t when t == typeof(ulong)  => BitConverter.ToUInt64(buffer),
                _                               => throw new NotSupportedException($"Unsupported enum underlying type: '{underlyingType.Name}'")
            };

            return (T)Enum.ToObject(typeof(T), value);
        }
    }
}
