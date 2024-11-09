struct PR2Item
{
    char profileFile[31];
    char modileFile[31];
    u16 type;
    float center;
    u16 unkxll;
    u16 unkxl2;
    u8 animsIds[4];
    u16 soundFXIds[4];
    u8 soundFXPitches[4];
};

struct PR2ItemFile
{
    u32 entriesCount;
    PR2Item entries[entriesCount];
};

PR2ItemFile file @ 0x00000000;