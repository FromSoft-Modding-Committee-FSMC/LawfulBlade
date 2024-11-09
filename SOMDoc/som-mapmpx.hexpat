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
};

MPXFile file @ 0x00000000;