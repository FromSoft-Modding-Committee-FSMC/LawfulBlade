struct SNDHeader
{
    u16 u16x00;         // ?? Normally 1
    u16 u16x02;         // ?? Normally 1
    u32 sampleRate1;    // Sample Rate #1. Normally 11025
    u32 sampleRate2;    // Sample Rate #2. Normally 22050
    u16 u16x0C;         // Bytes per sample? (Normally 2)
    u16 bitdepth;       // Bits Per Sample.  (Normally 16)
    u16 u16x10;         // Normally 0.
    u32 samplesBufferSize;
};


SNDHeader header @ 0x00000000;
u8 sampleBuffer[header.samplesBufferSize] @ 0x00000016;