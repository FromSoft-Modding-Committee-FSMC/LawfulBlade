using System.Text;

namespace LawfulBladeSDK.IO
{
    public partial class FileOutputStream
    {
        public void WriteFixedString(string value, int length) =>
            WriteFixedString(value, length, Encoding.ASCII);

        public void WriteFixedString(string value, int length, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(value);

            Span<byte> buffer = stackalloc byte[length];
            buffer.Clear();

            int copyLength = System.Math.Min(bytes.Length, length - 1); // -1 from length to force null termination

            bytes.AsSpan(0, copyLength).CopyTo(buffer);

            fstream.Write(buffer);
        }

        public void WriteTerminatedString(string value) =>
            WriteTerminatedString(value, Encoding.ASCII);

        public void WriteTerminatedString(string value, Encoding encoding)
        {
            // Get bytes of string
            byte[] bytes = encoding.GetBytes(value);

            // Write string...
            fstream.Write(bytes, 0, bytes.Length);
            fstream.WriteByte(0);   // Write extra byte for terminator...
        }

    }
}
