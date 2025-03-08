using LawfulBladeSDK.IO;
using Som2kRuntime.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Som2kRuntime.Formats
{
    internal class NpcPR2File
    {
        public List<NpcPR2Item> Items;

        public NpcPR2File(string filename)
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
                Items.Add(new NpcPR2Item
                {
                    prfName = fis.ReadFixedString(31, SJISEnc),
                    modelName = fis.ReadFixedString(31, SJISEnc),
                    u8x3e = fis.ReadU8(),
                    u8x3f = fis.ReadU8(),
                    f32x40 = fis.ReadF32(),
                    f32x44 = fis.ReadF32(),
                    f32x48 = fis.ReadF32(),
                    s16x4C = fis.ReadS16(),
                    s16x4E = fis.ReadS16(),
                    unkx50 = fis.ReadU32(),
                    unkx54 = fis.ReadU32(),
                    unkx58 = fis.ReadU32(),
                    hasExternalTexture = fis.ReadU8() == 1,
                    externalTextureName = fis.ReadFixedString(31, SJISEnc)
                });
                fis.Return();
            }
        }
    }


    internal struct NpcPR2Item
    {
        public string prfName;
        public string modelName;
        public byte u8x3e;
        public byte u8x3f;
        public float f32x40;
        public float f32x44;
        public float f32x48;
        public short s16x4C;
        public short s16x4E;
        public uint unkx50;
        public uint unkx54;
        public uint unkx58;
        public bool hasExternalTexture;
        public string externalTextureName;
    }
}