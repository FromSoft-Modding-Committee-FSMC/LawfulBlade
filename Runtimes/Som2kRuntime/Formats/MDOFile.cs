using LawfulBladeSDK.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Som2kRuntime.Formats
{
    internal class MDOFile
    {
        public static string[] GetTexturesFromMDO(string filename)
        {
            // Get S-JIS encoding
            Encoding SJISEnc = Encoding.GetEncoding(932);

            List<string> textureList = [];

            using FileInputStream fis = new FileInputStream(filename);

            // Read the number of textures from the FIS...
            int numTexture = fis.ReadS32();

            while (numTexture-- > 0)
                textureList.Add(Path.ChangeExtension(fis.ReadTerminatedString(SJISEnc), "txr"));

            return textureList.ToArray();
        }
    }
}
