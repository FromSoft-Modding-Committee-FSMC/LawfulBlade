//
// Sword of Moonlight MAP .prt
// Specification Version 1, Format Version 1
// Author(s): StolenBattenberg, Mick.
//

/* -------------- */
/* Specification  */
/* -------------- */
struct PRTMap
{
    char msmFileName[30];   // Name of the MSM file (render mesh)
    char msmFileNameSafeNT; // Null safety byte.
    u8 reservedx1F;
    
    char mhmFileName[30];   // Name of the MHM file (collision mesh)
    char mhmFileNameSafeNT; // Null safety byte.
    u8 reservedx3F;
    
    u8 blinders;            // According to mick, bits 0-3 = north,west,south,east respectively
    u8 ingameIconID;        // The ID of the in game map icon
    u8 flags;               // Bits 0, 1 control if the tile is a poison or damage trap.
    u8 reservedx43;
    
    char bmpFileName[30];   // Name of the BMP file (editor icon)
    char bmpFileNameSafeNT; // Null safety byte
    u8 reservedx63;
    
    char pieceName[30];     // Name of the piece
    char pieceNameSafeNT;   // Null safety byte
    u8 reservedx83;
    
    char reservedx84[96];
};

/* -------- */
/* Pattern  */
/* -------- */
PRTMap prt @ 0x00000000;