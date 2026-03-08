using LawfulBladeSDK.Extensions;
using System.Runtime.InteropServices;

namespace LawfulBladeSDK.Format.SoM
{
    public enum ItemType : ushort
    {
        Usable = 0,
        Weapon = 1,
        Armour = 2
    }

    public enum ItemArmourEquipType : byte
    {
        Helm = 0,
        Body = 1,
        Arms = 2,
        Boots = 3,
        Suit = 4,
        Shield = 5,
        Accessory = 6
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public unsafe struct ItemPRFDataUsable
    {
        [FieldOffset(0x00)] public uint unkx00;
        [FieldOffset(0x04)] public byte slotKeyID;
        [FieldOffset(0x05)] public byte unkx05;
        [FieldOffset(0x06)] public ushort unkx06;
        [FieldOffset(0x08)] public uint unkx08;
        [FieldOffset(0x0C)] public uint unkx0C;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public unsafe struct ItemPRFDataWeapon
    {
        [FieldOffset(0x00)] public byte swingAnimation;
        [FieldOffset(0x01)] public byte soundDelay;
        [FieldOffset(0x02)] public short soundID;
        [FieldOffset(0x04)] public byte hitWindowStart;
        [FieldOffset(0x05)] public byte hitWindowEnd;
        [FieldOffset(0x06)] public ushort attackArc;
        [FieldOffset(0x08)] public float attackRange;
        [FieldOffset(0x0C)] public ushort unkx0C;
        [FieldOffset(0x0E)] public byte magicWindowStart;
        [FieldOffset(0x0F)] public byte magicWindowEnd;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public unsafe struct ItemPRFDataArmour
    {
        [FieldOffset(0x00)] public ItemArmourEquipType equipType;
        [FieldOffset(0x01)] public byte unkx01;
        [FieldOffset(0x02)] public ushort unkx02;
        [FieldOffset(0x04)] public uint unkx04;
        [FieldOffset(0x08)] public uint unkx08;
        [FieldOffset(0x0C)] public uint unkx0C;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public unsafe struct ItemPRFData
    {
        [FieldOffset(0x00)] public ItemPRFDataUsable usable;
        [FieldOffset(0x00)] public ItemPRFDataWeapon weapon;
        [FieldOffset(0x00)] public ItemPRFDataArmour armour;
        [FieldOffset(0x00)] public fixed byte raw[16];
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public unsafe struct ItemPRF
    {
        [FieldOffset(0x00)] public fixed byte name[31];
        [FieldOffset(0x1F)] public fixed byte modelFile[31];
        [FieldOffset(0x3E)] public ItemType type;
        [FieldOffset(0x40)] public float menuElevationOffset;
        [FieldOffset(0X44)] public ushort menuTiltDegrees;
        [FieldOffset(0x46)] public ushort worldTiltDegrees;
        [FieldOffset(0x48)] public ItemPRFData data;

        public string Name
        {
            get
            {
                fixed (byte* fixedName = name)
                {
                    string result = EncodingExtensions.SJISEncoding.GetString(fixedName, 31);
                    return result[..result.IndexOf('\0')];
                }
            } 
        }

        public string ModelFile
        {
            get
            {
                fixed (byte* fixedModelFile = modelFile)
                {
                    string result = EncodingExtensions.SJISEncoding.GetString(fixedModelFile, 31);
                    return result[..result.IndexOf('\0')];
                }
            }
        }
    }
}
