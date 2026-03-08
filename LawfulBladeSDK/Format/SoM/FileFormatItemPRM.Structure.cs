using LawfulBladeSDK.Extensions;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace LawfulBladeSDK.Format.SoM
{
    public enum ItemEffectType : byte
    {
        None           = 0,
        Dark           = 1,
        Curse          = 2,
        HPRecover      = 3,
        HPDrain        = 4,
        HPAbsorb       = 5,
        MPRecover      = 6,
        MPDrain        = 7,
        MPAbsorb       = 8,
        StrengthUp     = 9,
        MagicUp        = 10,
        PoisonResist   = 11,
        ParalyseResist = 12,
        DarkResist     = 13,
        CurseResist    = 14,
        SlowResist     = 15
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public unsafe struct ItemPRMDataArmour
    {
        [FieldOffset(0x00)] public float weight;
        [FieldOffset(0x04)] public byte slashDef;
        [FieldOffset(0x05)] public byte smashDef;
        [FieldOffset(0x06)] public byte stabDef;
        [FieldOffset(0x07)] public byte fireDef;
        [FieldOffset(0x08)] public byte earthDef;
        [FieldOffset(0x09)] public byte windDef;
        [FieldOffset(0x0A)] public byte waterDef;
        [FieldOffset(0X0B)] public byte holyDef;
        [FieldOffset(0x0C)] public ItemEffectType effectType;
        [FieldOffset(0x0D)] public byte effectPotency;
        [FieldOffset(0x0E)] public fixed byte unkx0E[26];
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public unsafe struct ItemPRMDataWeapon
    {
        [FieldOffset(0x00)] public float weight;
        [FieldOffset(0x04)] public byte slashOff;
        [FieldOffset(0x05)] public byte smashOff;
        [FieldOffset(0x06)] public byte stabOff;
        [FieldOffset(0x07)] public byte fireOff;
        [FieldOffset(0x08)] public byte earthOff;
        [FieldOffset(0x09)] public byte windOff;
        [FieldOffset(0x0A)] public byte waterOff;
        [FieldOffset(0X0B)] public byte holyOff;
        [FieldOffset(0x0C)] public ItemEffectType effectType;
        [FieldOffset(0x0D)] public byte effectPotency;
        [FieldOffset(0x0E)] public sbyte magicID;
        [FieldOffset(0x0F)] public fixed byte unkx0E[25];
    }

    public enum ItemUsableType : byte
    {
        Normal     = 0,
        Recovery   = 1,
        DisplayMap = 2,
        Assess     = 3,
        Reveal     = 4
    }

    public enum ItemDisplayMapType : sbyte
    {
        None     = -1,
        Automap  =  0,
        Picture  =  1,
        DefinedA =  2,
        DefinedB =  3,
        DefinedC =  4
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public unsafe struct ItemPRMUsableDisplayMap
    {
        [FieldOffset(0x00)] public ItemDisplayMapType type;
        [FieldOffset(0x01)] public fixed byte pictureFile[31];

        public string PictureFile
        {
            get
            {
                fixed (byte* fixedName = pictureFile)
                {
                    string result = EncodingExtensions.SJISEncoding.GetString(fixedName, 31);
                    return result[..result.IndexOf('\0')];
                }
            }
        }
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public unsafe struct ItemPRMUsableRecovery
    {
        [FieldOffset(0x00)] public ushort hpRecover;
        [FieldOffset(0x02)] public ushort mpRecover;
        [FieldOffset(0x04)] public byte curePoison;
        [FieldOffset(0x05)] public byte cureParalyse;
        [FieldOffset(0x06)] public byte cureDark;
        [FieldOffset(0x07)] public byte cureCurse;
        [FieldOffset(0x08)] public byte cureSlow;
        [FieldOffset(0x09)] public fixed byte unkx09[23];
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public unsafe struct ItemPRMDataUsable
    {
        [FieldOffset(0x00)] public ItemUsableType usableType;
        [FieldOffset(0x01)] public byte unusable;
        [FieldOffset(0x02)] public byte reusable;
        [FieldOffset(0x03)] public fixed byte unkx03[5];

        [FieldOffset(0x08)] public ItemPRMUsableDisplayMap displayMapInfo;
        [FieldOffset(0x08)] public ItemPRMUsableRecovery recoveryInfo;
        [FieldOffset(0x08)] public fixed byte raw[32];
    }


    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public unsafe struct ItemPRMData
    {
        [FieldOffset(0x00)] public ItemPRMDataUsable usable;
        [FieldOffset(0x00)] public ItemPRMDataWeapon weapon;
        [FieldOffset(0x00)] public ItemPRMDataArmour armour;
        [FieldOffset(0x00)] public fixed byte raw[40];
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public unsafe struct ItemPRM
    {
        [FieldOffset(0x000)] public short pr2Index;
        [FieldOffset(0x002)] public fixed byte name[31];
        [FieldOffset(0x021)] public fixed byte description[241];
        [FieldOffset(0x112)] public fixed byte unkx112[16];
        [FieldOffset(0x122)] public byte priority;
        [FieldOffset(0x123)] public fixed byte unkx123[5];
        [FieldOffset(0x128)] public ItemPRMData data;
    }
}
