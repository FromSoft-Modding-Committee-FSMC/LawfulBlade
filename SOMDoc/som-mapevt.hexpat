struct EVTHeader
{
    u32 eventNum;	// always 1024?...
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
	char name[0x1F];
	u8   targetType; 			// FE = System?
	u16  targetID;	 			// 
	u8   u8x22;		 			// Unknown.
	u8   u8x23;	     			// Unknown.
	u16  interactionCone;		// The cone angle that the event can be triggered (relative to player?..)
	u16  u16x26;				// Unknown. Padding?
	f32  xwArea;				// East to West trigger coverage
	f32	 yhArea;				// North to South trigger coverage
	f32  radius;				// The radius of the circle trigger area?
	EVTConditional condition;	// When the event should be able to run
	EVTPage pages[12];			// The event pages.
};

struct EVTFile
{
    EVTHeader header;
	EVTDeclaration eventDecls[header.eventNum];
};

EVTFile file @ 0x00000000;