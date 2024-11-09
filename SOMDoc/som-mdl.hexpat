struct MDLHeader
{
    u8 flags;                   // 0x01 = Skinned animation,
    u8 numSkinnedAnimations;    // Number of skinned (skeletal) animations stored in the file
    u8 numVertexAnimations;     // Number of vertex morph (mime) animations stored in the file
    u8 numInternalTextures;     // Number of internal textures (PSX TIM format) stored in the file
    u8 numMeshes;               // Number of meshes
    u8 num0x05;                 // Number of unknown blocks
    u16 off0x06;                // Offset to ???
    u16 off0x08;                // Offset to ???
    u16 off0x0A;                // Offset to ???
    u16 off0x0C;                // Offset to ???
    u16 off0x0E;                // Offset to ???
};

struct MDLVertex    // Identical to PsyQ SVECTOR. This is a Q3.12 fixed point format vector.
{
    s16 X, Y, Z, W;
};

struct MDLNormal    // Identical to PsyQ SVECTOR. This is a Q3.12 fixed point format vector.
{
    s16 X, Y, Z, W;
};

// Primitive Types:

// I WON'T build a struct for it, but '0xFFFFFFFF' seems to signify a terminator.

struct MDLPrimHeader
{
    u16 totalCount;     // Includes terminator
    u16 usableCount;    // Includes renderable primitives only.
};

struct MDLPrim_F4
{
    u32 tag;            // (masks) 0xFF000000 = prim ID, 0x000000FF = mode (2C)
    u16 normalIndex;   
    u16 v0Index;
    u16 v1Index;
    u16 v2Index;
    u16 v3Index;
    u16 pad0x0E;        // PSX Primitives were all 4b aligned for speed - that followed through to MDL.
};

struct MDLPrims
{
    MDLPrimHeader header;   
};

struct MDLUV
{
    u8 U, V;
};

struct MDLUVSet4
{
    MDLUV uv0;
    u16 cba;
    MDLUV uv1;
    u16 tsb;
    MDLUV uv2;
    MDLUV uv3;
};

struct MDLUVBuffer
{
    u32 count;
    MDLUVSet4 UVs[count];
};


struct MDLMesh
{
    u32 vertexStart;        // Absolute offset to start of vertex data      (! SHIFT-RIGHT 2 format !)
    u32 vertexCount;        // 0-indexed count of vertices
    u32 normalStart;        // Absolute offset to start of normal data      (! SHIFT-RIGHT 2 format !)
    u32 normalCount;        // 0-indexed count of normals
    u32 primitiveStart;     // Absolute offset to start of primitive data   (! SHIFT-RIGHT 2 format !)
    u32 primitiveCount;     // 0-indexed count of primitives
    s32 scale;              // Unused TMD leftover.
    
    MDLVertex vertices[vertexCount] @ (0x10 + (vertexStart << 2));
    MDLNormal normals[normalCount] @ (0x10 + (normalStart << 2));
};

struct MDLFile
{
    MDLHeader header;
    MDLMesh meshes[header.numMeshes];
    MDLUVBuffer uvBuffer;
};

MDLFile file @ 0x00000000;