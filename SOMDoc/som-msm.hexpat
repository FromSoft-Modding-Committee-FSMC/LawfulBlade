/**
 * MSM Textures
**/
struct MSMTexturePath
{
    char filename[];
};

struct MSMTextureBlock
{
    u16 numTexture;
    MSMTexturePath texturePaths[numTexture];
};


/**
 * MSM Vector
**/
struct MSMVector3
{
    float x, y, z;
};

struct MSMVector2
{
    float x, y;
};


/**
 * MSM Vertices
**/
struct MSMVertex
{
    MSMVector3 position;
    MSMVector3 normal;
    MSMVector2 texcoord;
};

struct MSMVerticesBlock
{
    u16 numVertex;
    MSMVertex vertices[numVertex];
};

/**
 * MSM Primitives
**/
struct MSMPrimitive
{
    u16 u16x00;     // A number of children?
    s16 textureID;
    u16 numIndices;
    u16 indices[numIndices]; 
    
    if (u16x00 > 0)
    {
        MSMPrimitive children[u16x00];
    }
};


struct MSMPrimitiveRoot
{
    u16 u16x00;     // A number of children?
    s16 textureID;
    u16 numIndices;
    u16 indices[numIndices]; 
    
    MSMPrimitive children[u16x00-1];
};

struct MSMUnknownBlock
{
    u16 numx00;
};

/**
 * MSM Layout
**/
struct MSMFile
{
    MSMTextureBlock textures;
    MSMVerticesBlock vertices;
    MSMPrimitiveRoot chain;
    MSMPrimitiveRoot chain2;
    MSMUnknownBlock unknown;
};

MSMFile file @ 0x00000000;