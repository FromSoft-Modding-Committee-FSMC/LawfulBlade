#ifndef _SOMOBJECT_H
#define _SOMOBJECT_H

#include <stdint.h>
#include "sommath.h"

//
// Type Def
//
typedef struct 
{
	char name[31];
	int8_t u8x1F;
	float uniformScale;
	uint16_t profileId;
	uint16_t unkx26;
	uint32_t unkx28;
	uint32_t unkx2c;
	uint32_t unkx30;
	uint32_t unkx34;
} SomObjectParam;

typedef struct
{
	char name[31];
	char modelFile[31];
	uint16_t unkx3E;
	float colliderHeight;
	float colliderRadius1;
	float colliderRadius2;
	uint32_t unkx4C;
	uint8_t colliderType;
	uint8_t scrollingTexture;
	uint16_t type;
	int16_t spriteFxType;
	uint16_t spriteFxPeriod;
	uint32_t unkx58;
	uint32_t unkx5C;
	uint32_t unkx60;
	uint32_t unkx64;
	uint32_t unkx68;
	uint8_t keyItemId;
	uint8_t allowXZRotation;
} SomObjectProfile;

typedef struct
{
    uint16_t prmId;
    uint8_t zIndex; /* not sure */
    uint8_t animating;
    uint8_t visible;
    uint8_t unkx05;
    uint8_t unkx06;
    uint8_t unkx07;
    float f32x08;
    float f32x0c;
    float f32x10;
    float f32x14;
    float f32x18;
    float f32x1c;
    float startUniformScale;
    int8_t special[32];
} SomObjectDeclaration;

/*
typedef struct
{
    SomObjectDeclaration mapDecl;
    VECTOR3F position;
    undefined field16_0x50;
    undefined field17_0x51;
    undefined field18_0x52;
    undefined field19_0x53;
    undefined field20_0x54;
    undefined field21_0x55;
    undefined field22_0x56;
    undefined field23_0x57;
    undefined field24_0x58;
    undefined field25_0x59;
    undefined field26_0x5a;
    undefined field27_0x5b;
    float uniformScale;
    void* mdl;
    void* mdo;
    undefined field31_0x68;
    undefined field32_0x69;
    undefined field33_0x6a;
    undefined field34_0x6b;
    undefined field35_0x6c;
    undefined field36_0x6d;
    undefined field37_0x6e;
    undefined field38_0x6f;
    undefined field39_0x70;
    undefined field40_0x71;
    undefined field41_0x72;
    undefined field42_0x73;
    float f32x74;
    undefined field47_0x78;
    undefined field48_0x79;
    undefined field49_0x7a;
    undefined field50_0x7b;
    undefined field51_0x7c;
    undefined field52_0x7d;
    undefined field53_0x7e;
    undefined field54_0x7f;
    undefined field55_0x80;
    undefined field56_0x81;
    undefined field57_0x82;
    undefined field58_0x83;
    undefined field59_0x84;
    undefined field60_0x85;
    undefined field61_0x86;
    undefined field62_0x87;
    undefined field63_0x88;
    undefined field64_0x89;
    undefined field65_0x8a;
    undefined field66_0x8b;
    undefined field67_0x8c;
    undefined field68_0x8d;
    undefined field69_0x8e;
    undefined field70_0x8f;
    undefined field71_0x90;
    undefined field72_0x91;
    undefined field73_0x92;
    undefined field74_0x93;
    undefined field75_0x94;
    undefined field76_0x95;
    undefined field77_0x96;
    undefined field78_0x97;
    undefined field79_0x98;
    undefined field80_0x99;
    undefined field81_0x9a;
    undefined field82_0x9b;
    undefined field83_0x9c;
    undefined field84_0x9d;
    undefined field85_0x9e;
    undefined field86_0x9f;
    undefined field87_0xa0;
    undefined field88_0xa1;
    undefined field89_0xa2;
    undefined field90_0xa3;
    undefined field91_0xa4;
    undefined field92_0xa5;
    undefined field93_0xa6;
    undefined field94_0xa7;
    undefined field95_0xa8;
    undefined field96_0xa9;
    undefined field97_0xaa;
    undefined field98_0xab;
    undefined field99_0xac;
    undefined field100_0xad;
    undefined field101_0xae;
    undefined field102_0xaf;
    undefined field103_0xb0;
    undefined field104_0xb1;
    undefined field105_0xb2;
    undefined field106_0xb3;
    undefined field107_0xb4;
    undefined field108_0xb5;
    undefined field109_0xb6;
    undefined field110_0xb7;
} SomObjectInstance;
*/

#endif