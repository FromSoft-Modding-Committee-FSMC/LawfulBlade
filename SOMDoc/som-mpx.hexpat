struct MPXHeader
{
    u32 flags;
    char mapName[31];
    char mapNameNullByte;
    char musicFile[31];
    char musicFileNullByte;
};

struct MPXFile
{
    MPXHeader header;
};

MPXFile file @ 0x00000000;