struct EVTHeader
{
    u32 eventNum;
};

struct EVTConditional
{
	u16 mode;
	u16 counterId;
	u16 comparedValue;
	u16 comparisonType;
};

struct EVTPage
{
	u32 opsStart;
	EVTConditional condition;
};

struct EVTDeclaration
{
	char name[0x1E];
	char nullSafetyByte;
	u8   unk0x20;
};

struct EVTFile
{
    EVTHeader header;
};

EVTFile file @ 0x00000000;