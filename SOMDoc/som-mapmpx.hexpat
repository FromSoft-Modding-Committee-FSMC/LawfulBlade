struct MPXHeader
{
    u32 flags;
    char mapName[32];
    char musicFile[32];
    char mapImage1[32];
    char mapImage2[32];
    char mapImage3[32];
};

struct MPXCameraSettings
{
    float fov;
    float zNear;
    float zFar;
};

struct MPXFogSettings
{
    float fogDist;
    u8 fogColour[4];
};

struct MPXAmbientLight
{
    u8 colour[4];  
};

struct MPXDirectionalLight
{
    u8 colour[4];
    float direction[3];
};

struct MPXPlayerStart
{
    float position[3];
    float direction;
};

struct MPXObject
{
    u16 classID;
    u8 unkx02;
    u8 animating;
    u8 visible;
    u8 unkx05[3];
    float xyzPos[3];
    float unkVec[3];
    float scale;
    u8 flags[32];
};

struct MPXBlockObjects
{
    u32 count;
    MPXObject items[count];
};

struct MPXEnemyNpc
{
    u8 unkx00[4];
    float positionXYZ[3];
    float rotationXYZ[3];
    float scale;
    u16 classID;
    u8 unkx22[4];
    u8 droppedItemClassID;
    u8 unkx27[13]; 
};

struct MPXBlockEnemy
{
    u32 count;
    MPXEnemyNpc items[count];
};

struct MPXBlockNPC
{
    u32 count;
    MPXEnemyNpc items[count];
};

struct MPXItem
{
    u8 unkx00[28];
    u16 classID;
    u8 unkx1E[10];
};

struct MPXBlockItem
{
    u32 count;
    MPXItem items[count];
};

struct MPXTile
{
    u16 mhmID;
    u16 msmID;
    float elevation;
    u32 flags;  // msb:8,icon:8,:2,e:8,:4,rotation:2;
};

struct MPXTileMap
{
    u32 u32x00;
    u32 width;
    u32 height;
    
    MPXTile tiles[width * height]; 
};

struct MPXBSPUnknown1Item2
{
    u32 u32x00;
    u32 u32x04;
};

struct MPXBSPUnknown1Item1
{
    u32 unkx00;
    float xy1[2];
    float xy2[2];
    u32 count;
    MPXBSPUnknown1Item2 items[count];
};

struct MPXBSPUnknown1
{
    u32 count;
    
    MPXBSPUnknown1Item1 items[count];
};

struct MPXBSPUnknown2Item
{
    u32 unkx00;
    u32 unkx04;
    u32 unkx08;
    u32 unkx0c;
    float xy1[2];
    float xy2[2];
    u32 unkx20;
    u32 unkx24;
    u32 unkx28;
};

struct MPXBSPUnknown2
{
    u32 count;
    MPXBSPUnknown2Item item[count];
};

struct MPXBSPUnknown3Item
{
    float xy1[2];
    float xy2[2];
    u32 unkx10;
    u32 unkx14;
};

struct MPXBSPUnknown3
{
    u32 count;
    MPXBSPUnknown3Item item[count];
};

struct MPXBSPInfo
{
    MPXBSPUnknown1 bspInfo1;
    MPXBSPUnknown2 bspInfo2;
    MPXBSPUnknown3 bspInfo3;
    u8 junk[(bspInfo1.count+7)/8*bspInfo1.count];
};

struct MPXTexturePath
{
    char filename[];
};

struct MPXTextures
{
    u32 count;   
    MPXTexturePath textures[count];  
};

struct MPXVertex
{
    float position[3];
    float texcoord[2];
};

struct MPXVertexBlock
{
    u32 count;
    MPXVertex vertices[count];
};

struct MPXRenderMeshPrimitive
{
    u32 index;  // No fucking clue
    u8 unkx04;
    u8 unkx05;
    u8 unkx06;
    u8 unkx07;
};

struct MPXRenderMesh
{
    u32 u32x00;
    u16 u16x04;
    u16 indexCount;
    u16 primitiveCount;
    u16 indices[indexCount];
    MPXRenderMeshPrimitive primitives[primitiveCount];
};

struct MPXFile
{
    // Basic Data
    MPXHeader header;
    MPXCameraSettings cameraSettings;
    MPXFogSettings fogSettings;
    MPXAmbientLight ambientLight;
    MPXDirectionalLight directionalLights[3];
    u8 pad[4];
    MPXPlayerStart playerStart;
    MPXBlockObjects objects;
    MPXBlockEnemy enemies;
    MPXBlockNPC npcs;
    MPXBlockItem items;
    u32 skyMeshID;
    MPXTileMap tilemap;
    MPXBSPInfo bspInfo;
    MPXTextures textures;
    
    // MPX Mesh Data
    MPXVertexBlock vertices;
    
    // Render Meshes
    MPXRenderMesh renderMesh;
    
    
};

// ^ The meshes are not fully reversed yet...
// A[01 00 00 00] B[00 00] C[03 00] D[03 00]
// A[01 00 00 00] B[00 00] C[8A 00] D[4F 00]
// ^
// A = Unknown
// B = Unknown
// C = Number of vertex references?
// D = Number of weird primitives?..
//
// after are vertex references (u16), then the primitives...

// Then comes a list of bog standard MHM files... Very fuckin weird.

MPXFile file @ 0x00000000;