using LawfulBladeSDK.Extensions;
using System.Runtime.InteropServices;

namespace LawfulBladeSDK.Format.SoM
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public unsafe struct NpcPRF
    {
        [FieldOffset(0x000)] public fixed byte name[31];
        [FieldOffset(0x01F)] public fixed byte modelFile[31];
        [FieldOffset(0x03E)] public byte unkx3E;
        [FieldOffset(0x03F)] public byte unkx3F;
        [FieldOffset(0x040)] public float colliderHeight;
        [FieldOffset(0x044)] public float shadowRadius;
        [FieldOffset(0x048)] public float colliderRadius;
        [FieldOffset(0x04C)] public ushort unkx4C;
        [FieldOffset(0x04E)] public short effectID;
        [FieldOffset(0x050)] public byte effectControlPointAnchor;
        [FieldOffset(0x051)] public byte effectAnimationRate;
        [FieldOffset(0x052)] public short unkx52;
        [FieldOffset(0x054)] public float turnSpeed;
        [FieldOffset(0x058)] public byte standTalkForwardDelayFrames;
        [FieldOffset(0x059)] public byte standTalkSideDelayFrames;
        [FieldOffset(0x05A)] public byte sitTalkForwardDelayFrames;
        [FieldOffset(0x05B)] public byte sitTalkSideDelayFrames;
        [FieldOffset(0x05C)] public byte useExternalTexture;
        [FieldOffset(0x05D)] public fixed byte textureFile[31];

        // Lots more to implement, but we don't need it right now. TO-DO.

        public string Name
        {
            get
            {
                fixed (byte* fixedString = name)
                {
                    string result = EncodingExtensions.SJISEncoding.GetString(fixedString, 31);
                    return result[..result.IndexOf('\0')];
                }
            }

            set
            {
                fixed (byte* fixedString = name)
                {
                    Span<byte> buffer = new Span<byte>(fixedString, 31);
                    buffer.Clear();

                    byte[] encoded = EncodingExtensions.SJISEncoding.GetBytes(value ?? "");

                    int length = Math.Min(encoded.Length, 30);
                    encoded.AsSpan(0, length).CopyTo(buffer);

                    buffer[length] = 0;
                }
            }
        }

        public string ModelFile
        {
            get
            {
                fixed (byte* fixedString = modelFile)
                {
                    string result = EncodingExtensions.SJISEncoding.GetString(fixedString, 31);
                    return result[..result.IndexOf('\0')];
                }
            }

            set
            {
                fixed (byte* fixedString = modelFile)
                {
                    Span<byte> buffer = new Span<byte>(fixedString, 31);
                    buffer.Clear();

                    byte[] encoded = EncodingExtensions.SJISEncoding.GetBytes(value ?? "");

                    int length = Math.Min(encoded.Length, 30);
                    encoded.AsSpan(0, length).CopyTo(buffer);

                    buffer[length] = 0;
                }
            }
        }

        public string TextureFile
        {
            get
            {
                fixed (byte* fixedString = textureFile)
                {
                    string result = EncodingExtensions.SJISEncoding.GetString(fixedString, 31);
                    return result[..result.IndexOf('\0')];
                }
            }

            set
            {
                fixed (byte* fixedString = textureFile)
                {
                    Span<byte> buffer = new Span<byte>(fixedString, 31);
                    buffer.Clear();

                    byte[] encoded = EncodingExtensions.SJISEncoding.GetBytes(value ?? "");

                    int length = Math.Min(encoded.Length, 30);
                    encoded.AsSpan(0, length).CopyTo(buffer);

                    buffer[length] = 0;
                }
            }
        }
    }
}