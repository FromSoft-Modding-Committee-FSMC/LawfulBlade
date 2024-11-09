//
// Sword of Moonlight .MDO
// Specification Version 1, Format Version 1
// Author(s): StolenBattenberg, Mick.
//

#include <std/mem.pat>

/* -------------- */
/* Specification  */
/* -------------- */
struct MDOTexture
{
    char filename[];
};

struct MDOMaterial
{
    float DiffuseR; // Diffuse Colour
    float DiffuseG;
    float DiffuseB;
    float DiffuseA;
    float UnknownR; // Emissive?
    float UnknownG;
    float UnknownB;
    float UnknownX;
};

struct MDOVertex
{
    float Position[3];
    float Normal[3];
    float Texcoord[2];
};

struct MDOMesh
{
    u32 renderFlag;
    s16 textureIndex;
    s16 materialIndex;
    u16 numIndices;
    u16 numVertices;
    u32 indicesOfs;
    u32 verticesOfs;
    
    u16 indices[numIndices] @ indicesOfs;
    MDOVertex vertices[numVertices] @ verticesOfs;
};

struct MDOControlPoint
{
    float X;
    float Y;
    float Z;
};

struct MDOTextureBlock
{
    u32 count;
    MDOTexture textures[count];
};

struct MDOMaterialBlock
{
    u32 count;
    MDOMaterial materials[count];
};

struct MDOMeshBlock
{
    u32 count;
    MDOMesh meshes[count];
};

/* -------- */
/* Pattern  */
/* -------- */


MDOTextureBlock textureBlock @ 0x00000000;
MDOMaterialBlock materialBlock @ std::mem::align_to(4, $);
MDOControlPoint cps[4] @ $;
MDOMeshBlock meshBlock @ $;;