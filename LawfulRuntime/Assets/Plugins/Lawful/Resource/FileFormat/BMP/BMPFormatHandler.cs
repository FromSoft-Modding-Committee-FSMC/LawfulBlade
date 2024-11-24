using System;

using Unity.Collections;

using Lawful.IO;

namespace Lawful.Resource.FileFormat.BMP
{
    public class BMPFormatHandler : FileFormatHandler<TextureResource>
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
            name = "Microsoft [B]it[M]ap [P]icture (*.BMP)",
            description = "Raster graphics format from the age of dinosaurs",
            version = "1.0",
            authors = new string[] { "Microsoft" },
            extensions = new string[] { ".BMP" }
        };

        /// <summary>
        /// Validates the content of a stream as an BMP file.
        /// </summary>
        /// <param name="finStream">A stream containing the data to check</param>
        /// <returns>True if it is, false if it is not</returns>
        public override bool Validate(FileInputStream finStream)
        {
            return (finStream.ReadU16() == 0x4D42);
        }

        /// <summary>
        /// Parses an BMP file
        /// </summary>
        public override bool Load(FileInputStream finStream, in TextureResource resource)
        {
            // The stream is reused from the validation pass, so it's good practice to seek to the start
            finStream.SeekBegin(0);

            //
            // Reading
            //

            // BMP Header
            ushort bmpTag = finStream.ReadU16();
            uint fileSize = finStream.ReadU32();
            uint reservedx06 = finStream.ReadU32();
            uint imageOffset = finStream.ReadU32();

            // BMP Info
            uint infoSize = finStream.ReadU32();
            int width = finStream.ReadS32();
            int height = finStream.ReadS32();
            ushort planes = finStream.ReadU16();
            ushort bitsPerPixel = finStream.ReadU16();
            uint compression = finStream.ReadU32();
            uint imageSize = finStream.ReadU32();
            uint pixelsPerX = finStream.ReadU32();
            uint pixelsPerY = finStream.ReadU32();
            uint coloursUsed = finStream.ReadU32();
            uint coloursImportant = finStream.ReadU32();
            uint[] clut = null;

            if (compression != 0)
                throw new Exception("Compressed BMP files are not supported.");

            // BMP CLUT (only with bitsPerPixel <= 8)
            if (bitsPerPixel <= 8)
            {
                int clutColours = 1 << bitsPerPixel;
                clut = new uint[clutColours];

                for (int i = 0; i < clutColours; ++i)
                    clut[i] = finStream.ReadU32();
            }

            // Image Data
            finStream.SeekBegin(imageOffset);

            // We generate RGBA8 images
            int rowStride = (int)(4 * width);

            NativeArray<byte> imageBuffer = new ((int)(rowStride * height), Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

            unchecked   // We don't need overflow checking at the moment, thanks. gimmie the perf.
            {
                switch (bitsPerPixel)
                {
                    // Indexed Images
                    case <= 8:
                        int pixelsPerWord = (32 / bitsPerPixel);
                        int pixelMask     = (1 << bitsPerPixel) - 1;
                        int pixelShiftTop = bitsPerPixel * (pixelsPerWord - 1);

                        // Loop over every row...
                        for (int y = 0; y < height; ++y)
                        {
                            int rowPosition = rowStride * y;

                            // Loop over every column of pixels... 
                            for (int x = 0; x < width; x += pixelsPerWord)
                            {
                                // read and then rotate our pixel bytes in order for 32-bit reads to work.
                                uint pixelData = finStream.ReadU32();
                                     pixelData = (pixelData >> 24) | ((pixelData >> 8) & 0x0000FF00) | ((pixelData << 8) & 0x00FF0000) | (pixelData << 24);

                                uint clutData;

                                // Set shift register to the starting shift point, (1 bpp = 31, 4bpp = 28, 8bpp = 24)
                                int shiftRegister = pixelShiftTop;

                                // Which is smaller, the remaining pixels or the maximum amount of pixels per u32?
                                int widthMinX   = (width - x);
                                int packedCount = widthMinX + ((pixelsPerWord - widthMinX) & (pixelsPerWord - widthMinX) >> 31);
              
                                // now loop over each pixel packed into the 32-bit value, and unpack it.
                                for (int p = 0; p < packedCount; ++p)
                                {
                                    clutData = clut[(pixelData >> shiftRegister) & pixelMask];

                                    // Calculate this now so we don't need to do it per index
                                    int rowColumnOffset = rowPosition + (4 * (x + p));

                                    imageBuffer[rowColumnOffset + 0] = (byte)((clutData >> 16) & 0xFF); // r
                                    imageBuffer[rowColumnOffset + 1] = (byte)((clutData >>  8) & 0xFF);  // g
                                    imageBuffer[rowColumnOffset + 2] = (byte)((clutData >>  0) & 0xFF);  // b
                                    imageBuffer[rowColumnOffset + 3] = 0xFF; // a (doesn't exist in BMP, so assume max opacity)

                                    shiftRegister -= bitsPerPixel;
                                }
                            }
                        }
                        break;

                    // To do...
                    case 16: throw new Exception("16-bit BMP files are not supported.");
                    case 24: throw new Exception("24-bit BMP files are not supported.");
                    case 32: throw new Exception("32-bit BMP files are not supported.");
                }
            }
            
            //
            // Storing
            //

            resource.LoadPixels(imageBuffer, width, height);

            return true;
        }
    }
}