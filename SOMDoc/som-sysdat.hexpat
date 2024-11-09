struct SYSSequences
{
	char openSequenceMode;			// 0 = skip, 1 = video, 2 = slideshow
	char openSequenceVideo[31]; 	// Video file to play when 'openSequenceMode' is 1.
	char titleImage[31];			// Image file displayed on the title screen
	char introSequenceMode;			// See 'openSequenceMode'
	char introSequenceVideo[31];
	char gameEnd1SequenceMode;
	char gameEnd1Video[31];
	char gameEnd2SequenceMode;
	char gameEnd2Video[31];
	char gameEnd3SequenceMode;
	char gameEnd3Video[31];
	char staffSequenceMode;
	char staffSequenceVideo[31];	// Video file for the staff sequence
	char staffSequenceFinal[31];	// Image file displayed after the staff sequence ends
};

struct SYSPlayerSpeed
{
	float walkSpeed;
	float dashSpeed;
	u16 turnSpeed;
	u8 unkxA;
};

struct SYSClassName
{
    char text[15];
};

struct SYSClassNameBlock
{
    SYSClassName names[25];
    u16 magGraph[4];
    u16 strGraph[4];
};

struct SYSPlayerSpells
{
    u8 classIDs[32];
    u8 rankReq[32];
};

struct SYSConfig1
{
    u8 allowSaveInMenu;
    u8 forbidOverencumber;
    u8 compassType;
    u8 guageType;
    u8 unkx4;
    u8 menuStyle;
};

struct SYSMessageText
{
    char text[41];
};

struct SYSPlayerSetup
{
    u16 strength;
    u16 magic;
    u16 health;
    u16 mana;
    u32 gold;
    u32 experience;
    u8 level;
    u8 equipment[8];
    u8 inventory[251];
};

struct SYSUnknown
{
    char test[31];
};

struct SYSFile
{
	SYSSequences sequences;
	u8 enableDash;
	u8 unkxFF;
	SYSPlayerSpeed playerSpeed;
	SYSClassNameBlock classNames;
	SYSPlayerSpells spellData;
	SYSConfig1 config1;
	SYSMessageText messages[13];
	u8 goldSymbol[3];
	SYSPlayerSetup normal;
	SYSPlayerSetup debug;
	u8 startMap;
	SYSUnknown counterNames[1024];
	u8 unkx8319;
	u16 sounds[16];
	char menuBG[32];
	u8 unkx8383[6];
	SYSMessageText messagesExtra[3];
	u8 unkx83db[5];
};

SYSFile file @ 0x00000000;

// SysClassName test[25] @ 0x0000010B;
// SysUnknown test2[1024] @ 0x00000719;