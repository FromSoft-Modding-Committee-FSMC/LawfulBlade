struct PR2Object
{
    char objectName[31];
    char objectMeshFile[31];
    u8 unknown[46];
};

struct PR2Objects
{
    u32 count;
    PR2Object objects[count];
};

PR2Objects file @ 0x00000000;