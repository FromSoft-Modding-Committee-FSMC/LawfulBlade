using System.Runtime.InteropServices;

namespace LawfulBladeSDK.IO
{
    public partial class FileOutputStream
    {
        public void WriteStruct<T>(T value) where T : unmanaged
        {
            int structSize = Marshal.SizeOf<T>();

            Span<byte> buffer = stackalloc byte[structSize];

            // Unsafe win
            MemoryMarshal.Write(buffer, ref value);

            fstream.Write(buffer);
        }

        public void WriteEnum<T>(T value, Endianness endianness = Endianness.Little) where T : Enum
        {
            Type underlyingType = Enum.GetUnderlyingType(typeof(T));
            int enumSize = Marshal.SizeOf(underlyingType);

            Span<byte> buffer = stackalloc byte[enumSize];
            object rawValue = Convert.ChangeType(value, underlyingType);

            switch (rawValue)
            {
                case byte v: buffer[0] = v; break;
                case sbyte v: buffer[0] = unchecked((byte)v); break;
                case short v: BitConverter.TryWriteBytes(buffer, v); break;
                case ushort v: BitConverter.TryWriteBytes(buffer, v); break;
                case int v: BitConverter.TryWriteBytes(buffer, v); break;
                case uint v: BitConverter.TryWriteBytes(buffer, v); break;
                case long v: BitConverter.TryWriteBytes(buffer, v); break;
                case ulong v: BitConverter.TryWriteBytes(buffer, v); break;
                default:
                    throw new NotSupportedException(
                        $"Unsupported enum underlying type: '{underlyingType.Name}'");
            }

            if ((endianness == Endianness.Big && BitConverter.IsLittleEndian) || (endianness == Endianness.Little && !BitConverter.IsLittleEndian))
                buffer.Reverse();

            fstream.Write(buffer);
        }

        public T ReadEnum<T>(Endianness endianness = Endianness.Little) where T : Enum
        {
            Type underlyingType = Enum.GetUnderlyingType(typeof(T));
            int enumSize = Marshal.SizeOf(underlyingType);

            Span<byte> buffer = stackalloc byte[enumSize];
            fstream.ReadExactly(buffer);

            if ((endianness == Endianness.Big && BitConverter.IsLittleEndian) || (endianness == Endianness.Little && !BitConverter.IsLittleEndian))
                buffer.Reverse();

            object value = underlyingType switch
            {
                Type t when t == typeof(byte) => buffer[0],
                Type t when t == typeof(sbyte) => unchecked((sbyte)buffer[0]),
                Type t when t == typeof(short) => BitConverter.ToInt16(buffer),
                Type t when t == typeof(ushort) => BitConverter.ToUInt16(buffer),
                Type t when t == typeof(int) => BitConverter.ToInt32(buffer),
                Type t when t == typeof(uint) => BitConverter.ToUInt32(buffer),
                Type t when t == typeof(long) => BitConverter.ToInt64(buffer),
                Type t when t == typeof(ulong) => BitConverter.ToUInt64(buffer),
                _ => throw new NotSupportedException($"Unsupported enum underlying type: '{underlyingType.Name}'")
            };

            return (T)Enum.ToObject(typeof(T), value);
        }

    }
}
