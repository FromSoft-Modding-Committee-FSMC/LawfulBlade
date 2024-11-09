//
// Sword of Moonlight .MHM
// Specification Version 1, Format Version 1
// Author(s): StolenBattenberg, Mick.
//

/* -------------- */
/* Specification  */
/* -------------- */
struct MHMHeader
{
    u32 numVertex;
    u32 numNormal;
    u32 numPacket;
    u32 numAAXZ;   	// Axis Aligned Walls	(Check for normals facing forward,backward,left or right)
    u32 numAAY;    	// Axis Aligned Floors	(Check for normals facing up, down)
    u32 numNAAXYZ; 	// Non Axis Aligned		(Any that fail above checks end up in here)
};

struct MHMVector
{
    float X;
    float Y;
    float Z;
};

struct MHMPacket
{
    u32 resolutionMode;   // 0 for NAAXYZ type packets, 1 for AAXZ and AAY type packets
    MHMVector aabbMin;
    MHMVector aabbMax;
    u32 normalIndex;
    u32 numIndices;
};

/* -------- */
/* Pattern  */
/* -------- */
struct MHMFile
{
	MHMHeader header;
	MHMVector vertices[header.numVertex];
	MHMVector normals[header.numNormal];
	MHMPacket packets[header.numPacket];	// Packets are stored in the order: AAXZ, AAY, NAAXYZ
	
	// You must calculate 'totalNumIndices' by going though each packet and adding up the 'numIndices' fields
	s32 i = 0;
	s32 totalNumIndices = 0;
	while (i < header.numPacket)
	{
		totalNumIndices += packets[i].vertexIndexCount;
		i += 1;
	}
	
	u32 indices[totalNumIndices];
};

MHMFile file @ 0x00000000;