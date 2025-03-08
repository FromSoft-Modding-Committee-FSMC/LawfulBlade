using LawfulBladeSDK.IO;
using Som2kRuntime.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Som2kRuntime.Formats
{
    internal class ItemPR2File
    {
        public List<ItemPR2Item> Items;

        public ItemPR2File(string filename) 
        {
            // Get S-JIS encoding
            Encoding SJISEnc = Encoding.GetEncoding(932);

            using FileInputStream fis = new FileInputStream(filename);

            // Read Items
            Items = [];

            // We need to get the number of items to read first...
            int itemCount = fis.ReadS32();

            for (int i = 0; i < itemCount; ++i)
            {
                Items.Add(new ItemPR2Item
                {
                    name      = fis.ReadFixedString(31),
                    modelFile = fis.ReadFixedString(31),
                    type = fis.ReadU16(),
                    center = fis.ReadF32(),
                    u16x44 = fis.ReadU16(),
                    u16x46 = fis.ReadU16(),
                    animIds = fis.ReadU8Array(4),
                    soundIDs = fis.ReadU16Array(4),
                    soundPitches = fis.ReadU8Array(4)
                });
            }
        }
    }

    internal struct ItemPR2Item
    {
        public string name;
        public string modelFile;
        public ushort type;
        public float center;
        public ushort u16x44;
        public ushort u16x46;
        public byte[] animIds;
        public ushort[] soundIDs;
        public byte[] soundPitches;
    }
}
