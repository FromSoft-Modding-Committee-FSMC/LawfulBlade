using LawfulBladeSDK.Extensions;
using System.Runtime.InteropServices;

namespace LawfulBladeSDK.Format.SoM
{
    /// <summary>
    /// Sequence Data Type
    /// </summary>
    public enum SysDatSequenceMode : byte
    {
        Unused    = 0,
        Video     = 1,
        Slideshow = 2
    }

    /// <summary>
    /// Sequence Data
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public unsafe struct SysDatSequence
    {
        [FieldOffset(0x00)] public SysDatSequenceMode sequenceMode;
        [FieldOffset(0x01)] public fixed byte sequenceFile[31];

        public string SequenceFile
        {
            get
            {
                fixed (byte* p = sequenceFile)
                {
                    string result = EncodingExtensions.SJISEncoding.GetString(p, 31);
                    return result[..result.IndexOf('\0')];
                }
            }

            set
            {
                fixed (byte* p = sequenceFile)
                {
                    Span<byte> buffer = new Span<byte>(p, 31);
                    buffer.Clear();

                    byte[] encoded = EncodingExtensions.SJISEncoding.GetBytes(value ?? "");

                    int length = Math.Min(encoded.Length, 30);
                    encoded.AsSpan(0, length).CopyTo(buffer);

                    buffer[length] = 0;
                }
            }
        }
    }

    /// <summary>
    /// Player Setting A
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public unsafe struct SysDatPlayerSettingA
    {
        [FieldOffset(0x00)] public float walkSpeed;
        [FieldOffset(0x04)] public float dashSpeed;
        [FieldOffset(0x08)] public ushort turnSpeed;
        [FieldOffset(0x0A)] public byte levelTableID;
    }

    /// <summary>
    /// Player class data
    /// </summary>
    [StructLayout (LayoutKind.Explicit, Pack = 1)]
    public unsafe struct SysDatClassSetting
    {
        [FieldOffset(0x000)] public fixed byte classNames[15 * 25];
        [FieldOffset(0x177)] public fixed ushort strengthThresholds[4];
        [FieldOffset(0X17F)] public fixed ushort magicThresholds[4];

        public string GetClassName(int index)
        {
            // Bounds Check
            if (index < 0 && index >= 25)
                throw new ArgumentOutOfRangeException(nameof(index));

            fixed (byte* p = &classNames[15 * index])
            {
                string result = EncodingExtensions.SJISEncoding.GetString(p, 31);
                return result[..result.IndexOf('\0')];
            }
        }

        public void SetClassName(int index, string className)
        {
            // Bounds Check
            if (index < 0 && index >= 25)
                throw new ArgumentOutOfRangeException(nameof(index));

            fixed (byte* p = &classNames[15 * index])
            {
                Span<byte> buffer = new Span<byte>(p, 31);
                buffer.Clear();

                byte[] encoded = EncodingExtensions.SJISEncoding.GetBytes(className ?? string.Empty);

                int length = Math.Min(encoded.Length, 30);
                encoded.AsSpan(0, length).CopyTo(buffer);

                buffer[length] = 0;
            }
        }
    }

    /// <summary>
    /// Player magic setting
    /// </summary>
    [StructLayout (LayoutKind.Explicit, Pack = 1)]
    public unsafe struct SysDatMagicSetting
    {
        [FieldOffset(0x00)] public fixed sbyte spellIDs[32];
        [FieldOffset(0x20)] public fixed byte levelRequired[32];
    }

    /// <summary>
    /// Menu setting
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct SysDatMenuSetting
    {
        [FieldOffset(0x00)] public byte allowSaveInMenu;
        [FieldOffset(0x01)] public byte enableEncumbrance;
        [FieldOffset(0x02)] public byte compassType;
        [FieldOffset(0x03)] public byte gaugeType;
        [FieldOffset(0x04)] public byte padx4;
        [FieldOffset(0x05)] public byte menuSkinType;
    }
}