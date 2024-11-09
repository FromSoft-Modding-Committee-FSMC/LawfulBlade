//
// Sword of Moonlight (Runtime) ITEM.PRM
// Specification Version 1, Format Version 1
// Author(s): StolenBattenberg, Mick.
//

/* -------------- */
/* Specification  */
/* -------------- */
struct PRMItemRuntime
{
    s16 prfID;              // ID of the entry in PR2 file  
    char name[31];          // Name of the item    
    char description[241];  // Description of the item.
    u8 unk[18];
    float weight;
    float weight2;
    u8 unk2[10];
    u8 techniqueID1;
    u8 unk3[25];
};

/* -------- */
/* Pattern  */
/* -------- */
PRMItemRuntime runtimeItems[250] @ 0x00000000;



// @ 122 (Normal, Curcial Option)

// @ 128 (Armour/Weapon: Weight, Float)
// @ 12C (Armour/Weapon: Slash Def, byte)
// @ 12D (Armour/Weapon: Smash Def, byte)
// @ 12E (Armour/Weapon: Stab Def, byte)
// @ 12F (Armour/Weapon: Flame Def, byte)
// @ 130 (Armour/Weapon: Earth Def, byte)
// @ 131 (Armour/Weapon: Wind Def, byte)
// @ 132 (Armour/Weapon: Water Def, byte)
// @ 133 (Armour/Weapon: Holy Def, byte)
// @ 134 (Armour/Weapon: Status Effect, byte)
// @ 135 (Armour/Weapon: Status Potency?, byte)
// @ 136 (Weapon: Sword Magic, byte)




// 250