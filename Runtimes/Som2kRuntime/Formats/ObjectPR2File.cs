using LawfulBladeSDK.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Som2kRuntime.Formats
{
    internal class ObjectPR2File
    {
        public List<ObjectPR2Item> Items;

        public ObjectPR2File(string filename)
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
                Items.Add(new ObjectPR2Item
                {
                    name = fis.ReadFixedString(31),
                    modelFile = fis.ReadFixedString(31),
                });

                fis.SeekRelative(0x2E);
            }
        }
    }

    internal struct ObjectPR2Item
    {
        public string name;
        public string modelFile;

        // There is a lot more, but we will just skip it for now
    }
}