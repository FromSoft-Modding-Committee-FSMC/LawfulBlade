using System;

using Unity.Collections;

using Lawful.IO;

namespace Lawful.Resource.FileFormat.WAV
{
    public class WAVFormatHandler : FileFormatHandler<AudioResource>
    {
        public override FileFormatCapabilities Capabilities => new()
        {
            allowExport  = false,
            allowImport  = true,
            deprecated   = false,
            experimental = false
        };

        public override FileFormatMetadata Metadata => new()
        {
            name        = "[WAV]eform (*.SND)",
            description = "Basic PCM audio format created by Microsoft",
            version     = "3.0",
            authors     = new string[] { "Microsoft" },
            extensions  = new string[] { ".WAV", ".WAVE" }
        };

        /// <summary>
        /// Validates the content of a stream as an WAV file
        /// </summary>
        /// <param name="finStream">A stream containing the data to check</param>
        /// <returns>True if it is, false if it is not</returns>
        public override bool Validate(FileInputStream finStream)
        {
            // We must have both a RIFF and WAVE chunk to consider the file valid.
            bool valid = true;
                 valid &= (finStream.ReadU32() == 0x46464952);  // Check 'RIFF'
            finStream.SeekRelative(4);                          // Skip the chunk size
                 valid &= (finStream.ReadU32() == 0x45564157);  // Check 'WAVE'

            return valid;
        }

        public override bool Load(FileInputStream finStream, in AudioResource resource)
        {
            // The stream is reused from the validation pass, so it's good practice to seek to the start
            finStream.SeekBegin(0);

            //
            // Reading
            // 

            // RIFF Chunk
            uint riffChunkName = finStream.ReadU32();
            uint riffChunkSize = finStream.ReadU32();

            // WAVE Chunk
            uint waveChunkName = finStream.ReadU32();

            // Store our byte buffer...
            ushort formatCode = 0;
            ushort channelCount = 0;
            uint sampleRate = 0;
            uint avgSampleRate = 0;
            ushort blockAlign = 0;
            ushort bitsPerSample = 0;
            byte[] byteBuffer = null;

            // Chunk reading loop...
            while (!finStream.EndOfStream())
            {
                // Read the chunk tag
                uint genericChunkName = finStream.ReadU32();
                uint genericChunkSize = finStream.ReadU32();
                
                switch (genericChunkName)
                {
                    case 0x20746D66:    // 'fmt '

                        // Read the fmt packet
                        formatCode = finStream.ReadU16();
                        channelCount = finStream.ReadU16();
                        sampleRate = finStream.ReadU32();
                        avgSampleRate = finStream.ReadU32();
                        blockAlign = finStream.ReadU16();
                        bitsPerSample = finStream.ReadU16();

                        // fmt can come in 16, 18 and 40 byte sizes. We skip any bytes past 16 because they are pointless to this implementation
                        if ((genericChunkSize - 16) >= 0)
                            finStream.SeekRelative((genericChunkSize - 16));

                        break;

                    case 0x61746164:    // 'data'

                        // Read the samples
                        byteBuffer = finStream.ReadU8Array((int)genericChunkSize);

                        // If generic chunk size is odd, we must skip one additional byte.
                        if ((genericChunkSize & 0b1) != 0)
                            finStream.SeekRelative(1);

                        // When we have found and read the data chunk, we can assume we are ready to go.
                        goto ReadComplete;

                    // We only care about the 'fmt' and 'data' chunks. we skip all others.
                    default: finStream.SeekRelative(genericChunkSize); break;
                }
            }

            //
            // Parsing
            //
            ReadComplete:

            // If byte buffer is null, we somehow missed our data chunk...
            if (byteBuffer == null)
                throw new Exception("Failed to find a data block inside the WAV file!");

            // Check to see if we're in a compatible format
            if (formatCode != 0x0001 && formatCode != 0x0003)   // WAVE_FORMAT_PCM, WAVE_FORMAT_IEEE_FLOAT
                throw new Exception($"Invalid WAV format: 0x{formatCode:X4}!");

            // First calculate the number of bytes per sample, and the number of total samples
            int bytesPerSample = bitsPerSample / 8;
            int sampleLength   = byteBuffer.Length / bytesPerSample;

            // Now we can start parsing our samples into floats
            NativeArray<float> sampleBuffer = new(sampleLength, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

            switch (formatCode)
            {
                // WAVE_FORMAT_PCM
                case 0x0001:
                    // Easy as hell. we just put them inside the sample buffer
                    for (int i = 0; i < sampleLength; ++i)
                    {
                        switch (bytesPerSample)
                        {
                            case 1:
                                sampleBuffer[i] = (byteBuffer[i] / (float)sbyte.MaxValue);
                                break;

                            case 2:
                                sampleBuffer[i] = BitConverter.ToInt16(byteBuffer, (bytesPerSample) * i) / (float)short.MaxValue;
                                break;

                            case 4:
                                sampleBuffer[i] = BitConverter.ToInt32(byteBuffer, (bytesPerSample) * i) / (float)int.MaxValue;
                                break;
                        }
                    }              
                    break;

                // WAVE_FORMAT_IEEE_FLOAT
                case 0x0003:
                    // Easy as hell. we just put them inside the sample buffer
                    for (int i = 0; i < sampleLength; ++i)
                        sampleBuffer[i] = BitConverter.ToSingle(byteBuffer, (bytesPerSample) * i);
                    break;
            }

            //
            // Storing
            //

            // Now we must store our parsed data in our resource.
            resource.LoadSamples(sampleBuffer, (int)sampleRate, channelCount);

            return true;
        }
    }
}