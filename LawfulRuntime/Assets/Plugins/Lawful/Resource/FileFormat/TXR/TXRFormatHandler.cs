using System;

using Unity.Collections;

using Lawful.IO;

namespace Lawful.Resource.FileFormat.TXR
{
    public class TXRFormatHandler : FileFormatHandler<TextureResource>
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
            name = "Sword of Moonlight [T]e[X]tu[R]e (*.TXR)",
            description = "Awful raster image format, based on BMP",
            version = "1.0",
            authors = new string[] { "FromSoftware" },
            extensions = new string[] { ".TXR" }
        };

        /// <summary>
        /// Validates the content of a stream as an TXR file.
        /// </summary>
        /// <param name="finStream">A stream containing the data to check</param>
        /// <returns>True if it is, false if it is not</returns>
        public override bool Validate(FileInputStream finStream)
        {
            // The first 8 bytes are width/height, we can't use them for validation.
            finStream.SeekBegin(8);

            return finStream.ReadU32() switch
            {
                // Check agaisnt all valid BPPs... It's the best we can do.
                0x01 or 0x04 or 0x08 or 0x10 or 0x18 or 0x20 => true,
                _                                            => false
            };
        }

        /// <summary>
        /// Parses an TXR file
        /// </summary>
        public override bool Load(FileInputStream finStream, in TextureResource resource)
        {
            // The stream is reused from the validation pass, so it's good practice to seek to the start
            finStream.SeekBegin(0);

            //
            // Reading
            //

            // TXR has a minimal header...
            int txrWidth  = finStream.ReadS32();
            int txrHeight = finStream.ReadS32();
            int txrBPP    = finStream.ReadS32();
            int txrMips   = finStream.ReadS32();    // We don't bother reading these. We can generate better mip maps in 2024

            // Calculate the size of a row
            int rowStride = 4 * txrWidth;

            // We store our loaded data here
            NativeArray<byte> imageBuffer = new((int)(rowStride * txrHeight), Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

            switch (txrBPP)
            {
                // Indexed
                case <= 8:
                    // Indexed images have a CLUT present. Read the CLUT.
                    int txrClutSize = 1 << txrBPP;
                    uint[] txrClut  = new uint[txrClutSize];

                    for (int i = 0; i < txrClutSize; ++i)
                        txrClut[i] = finStream.ReadU32();

                    // Calculate and cache some information about the TXR.
                    int pixelsPerByte = (8 / txrBPP);               // 1, 2 or 8 pixels.
                    int pixelMask = (1 << txrBPP) - 1;              // 0x1, 0x3 or 0x1F
                    int pixelShift = txrBPP * (pixelsPerByte - 1);  // Calculate our starting shift amount
                    
                    uint pixelData, clutData;
                    int rowPosition, rowColumnOffset, shiftRegister;
                    int widthMinX, packedCount;

                    for (int y = 0; y < txrHeight; ++y)
                    {
                        rowPosition = rowStride * y;

                        for (int x = 0; x < txrWidth; x += pixelsPerByte)
                        {
                            // We have to load using U8 because TXR is unaligned
                            pixelData = finStream.ReadU8();

                            shiftRegister = pixelShift;

                            // Which is smaller, the remaining pixels or the maximum amount of pixels per u32?
                            widthMinX = (txrWidth - x);
                            packedCount = widthMinX + ((pixelsPerByte - widthMinX) & (pixelsPerByte - widthMinX) >> 31);

                            for (int p = 0; p < packedCount; ++p)
                            {
                                clutData = txrClut[(pixelData >> shiftRegister) & pixelMask];

                                rowColumnOffset = rowPosition + (4 * (x + p));

                                imageBuffer[rowColumnOffset + 0] = (byte)((clutData >> 16) & 0xFF); // r
                                imageBuffer[rowColumnOffset + 1] = (byte)((clutData >> 8) & 0xFF);  // g
                                imageBuffer[rowColumnOffset + 2] = (byte)((clutData >> 0) & 0xFF);  // b
                                imageBuffer[rowColumnOffset + 3] = 0xFF;                            // a, assume max opacity

                                shiftRegister -= txrBPP;
                            }
                        }
                    }
                    break; 

                case 16: throw new Exception("16-bit TXR files are unsupported.");

                // Direct 24-Bit
                case 24:
                    for (int y = 0; y < txrHeight; ++y)
                    {
                        for (int x = 0; x < txrWidth; ++x)
                        {
                            imageBuffer[(rowStride * y) + (4 * x) + 2] = finStream.ReadU8();
                            imageBuffer[(rowStride * y) + (4 * x) + 1] = finStream.ReadU8();
                            imageBuffer[(rowStride * y) + (4 * x) + 0] = finStream.ReadU8();
                            imageBuffer[(rowStride * y) + (4 * x) + 3] = 0xFF;
                        }
                    }
                    break;

                case 32: throw new Exception("32-bit TXR files are unsupported.");
            }


            //
            // Storing
            //

            resource.LoadPixels(imageBuffer, txrWidth, txrHeight);

            return true;
        }
    }
}