using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawfulBlade.Core
{
    public class LargeAddressAware
    {
        /// <summary>
        /// Sets the LAA flag for an executable file
        /// </summary>
        public static void SetLAAFlag(string filepath)
        {
            // Open the executable file...
            using FileStream fs = new(filepath, FileMode.Open, FileAccess.ReadWrite);

            byte[] readBuff = new byte[4];
            fs.Seek(0x3C, SeekOrigin.Begin);

            // Read the IMAGE_DOS_HEADER->e_lfanew
            fs.ReadExactly(readBuff, 0, 4);

            int e_lfanew = BitConverter.ToInt32(readBuff, 0) + 22;

            // Read IMAGE_FILE_HEADERS->Characteristics
            fs.Seek(e_lfanew, SeekOrigin.Begin);
            fs.ReadExactly(readBuff, 0, 2);


            short characteristics = BitConverter.ToInt16(readBuff, 0);

            // Check if it's a 32-bit executable
            if ((characteristics & 0x0102) != 0x0102)
                return;


            // Set the LAA flag...
            characteristics |= 0x0020;

            fs.Seek(e_lfanew, SeekOrigin.Begin);
            readBuff = BitConverter.GetBytes(characteristics);
            fs.Write(readBuff, 0, 2);

            fs.Flush();
        }
    }
}
