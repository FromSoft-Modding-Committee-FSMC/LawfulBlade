using UnityEngine;

namespace Lawful.IO
{
    public partial class FileInputStream
    {
        /// <summary>
        /// Reads a Vector3 from the stream
        /// </summary>
        public Vector3 ReadVector3(Endianness endianness = Endianness.Little)
        {
            // Read raw components from the stream
            float vx = ReadF32(endianness);
            float vy = ReadF32(endianness);
            float vz = ReadF32(endianness);

            // Return our new vector
            return new Vector3(vx, vy, vz);
        }

        /// <summary>
        /// Reads a Vector2 from the stream
        /// </summary>
        public Vector2 ReadVector2(Endianness endianness = Endianness.Little)
        {
            // Read raw components from the stream
            float vx = ReadF32(endianness);
            float vy = ReadF32(endianness);

            // Return our new vector
            return new Vector2(vx, vy);
        }

        /// <summary>
        /// Reads a colour with byte components from the stream
        /// </summary>
        public Color ReadColourRGBX32()
        {
            float cr = (1f / 255f) * ReadU8();
            float cg = (1f / 255f) * ReadU8();
            float cb = (1f / 255f) * ReadU8();
            ReadU8();

            return new Color(cr, cg, cb);
        }
    }
}
