using System;

using Unity.Collections;

using Lawful.IO;

namespace Lawful.Resource.FileFormat.SND
{
    public class SNDFormatHandler : FileFormatHandler<AudioResource>
    {
        public override FileFormatCapabilities Capabilities => new()
        {
            allowExport  = false,
            allowImport  = true,
            deprecated   = false,   // Sadly...
            experimental = false    
        };

        public override FileFormatMetadata Metadata => new()
        {
            name        = "Sword of Moonlight [S]ou[ND] (*.SND)",
            description = "Proprietary audio file format created for Sword of Moonlight: King's Field Making Tool",
            version     = "1.0",
            authors     = new string[] { "FromSoftware" },
            extensions  = new string[] { ".SND" }
        };

        /// <summary>
        /// Validates the content of a stream as an SND file.
        /// </summary>
        /// <param name="finStream">A stream containing the data to check</param>
        /// <returns>True if it is, false if it is not</returns>
        public override bool Validate(FileInputStream finStream) => true;   // TO-DO

        /// <summary>
        /// Parses an SND file
        /// </summary>
        public override bool Load(FileInputStream finStream, in AudioResource resource)
        {
            // The stream is reused from the validation pass, so it's good practice to seek to the start
            finStream.SeekBegin(0);

            //
            // Reading
            //

            // Read the SND Header
            ushort u16x00    = finStream.ReadU16();     // Normally 1.
            ushort u16x02    = finStream.ReadU16();     // Normally 1.
            uint sampleRate1 = finStream.ReadU32();     // Normally 11025hz ?..
            uint sampleRate2 = finStream.ReadU32();     // Normally 22050hz ? No idea why there's two sample rates...
            ushort u16x0C    = finStream.ReadU16();     // Bytes per sample? Normally 2.
            ushort u16x0E    = finStream.ReadU16();     // Bits per sample? Normally 16...
            ushort u16x10    = finStream.ReadU16();     // Normally 0.
            uint byteLength  = finStream.ReadU32();     // Length of the sample buffer in bytes

            // Read the SND Sample Buffer
            byte[] byteBuffer = finStream.ReadU8Array((int)byteLength);

            //
            // Parsing
            //

            // First calculate how many samples we have.
            int sampleLength = (int)(byteLength / 2);

            NativeArray<float> sampleBuffer = new (sampleLength, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

            // Loop by sample length
            for (int i = 0; i < sampleLength; ++i)
                sampleBuffer[i] = BitConverter.ToInt16(byteBuffer, 2 * i) / (float)short.MaxValue;

            //
            // Storing
            //

            // Now we must store our parsed data in our resource.
            resource.LoadSamples(sampleBuffer, (int)sampleRate1, 1);

            return true;
        }
    }
}