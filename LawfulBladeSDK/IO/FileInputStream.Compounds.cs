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
    }
}
