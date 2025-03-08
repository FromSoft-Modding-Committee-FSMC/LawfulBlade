using LawfulBladeSDK.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Som2kRuntime.Formats
{
    internal class EnemyPR2File
    {
        public List<EnemyPR2Item> Items;

        public EnemyPR2File(string filename) 
        {
            // Get S-JIS encoding
            Encoding SJISEnc = Encoding.GetEncoding(932);

            using FileInputStream fis = new FileInputStream(filename);

            // Read Items
            Items = [];

            for (int i = 0; i < 1024; ++i)
            {
                // Get the position
                uint position = fis.ReadU32();

                // If position is 0, it's an empty entry
                if (position == 0)
                    continue;

                // Now read the item...
                fis.Jump(position);
                Items.Add(new EnemyPR2Item
                {
                    prfName             = fis.ReadFixedString(31, SJISEnc),
                    modelName           = fis.ReadFixedString(31, SJISEnc),
                    u8x3e               = fis.ReadU8(),
                    u8x3f               = fis.ReadU8(),
                    hasExternalTexture  = fis.ReadU8() == 1,
                    externalTextureName = fis.ReadFixedString(31, SJISEnc)
                });
                fis.Return();
            }    
        }
    }

    internal struct EnemyPR2Item
    {
        public string prfName;
        public string modelName;
        public byte u8x3e;
        public byte u8x3f;
        public bool hasExternalTexture;
        public string externalTextureName;
    }
}
