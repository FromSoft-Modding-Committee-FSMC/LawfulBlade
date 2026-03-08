using System.Runtime.InteropServices;

namespace LawfulBladeSDK.Format.SoM
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public unsafe struct MDOMaterial
    {
        [FieldOffset(0x00)] public fixed float diffuseR8G8B8A8[4];
        [FieldOffset(0x10)] public fixed float emissiveR8G8B8X8[4];
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct MDOControlPoint
    {
        [FieldOffset(0x00)] public float x;
        [FieldOffset(0x04)] public float y;
        [FieldOffset(0x08)] public float z;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public unsafe struct MDOVertex
    {
        [FieldOffset(0x00)] public fixed float position[3];
        [FieldOffset(0x0C)] public fixed float normal[3];
        [FieldOffset(0x18)] public fixed float texcoord[2];
    }
}
