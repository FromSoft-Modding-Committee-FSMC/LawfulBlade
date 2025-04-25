using LawfulBladeSDK.IO;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ParamPro.Format
{
    public class EditorNpcPRF
    {
        /// <summary>The name of the NPC Profile</summary>
        public string Name;

        /// <summary>The name of the model file used for the NPC</summary>
        public string ModelFile;

        /// <summary>Unknown. Usually 0x0100...</summary>
        public ushort Unkx3E;

        /// <summary>The height of the cylinder collider</summary>
        public float ColliderHeight;

        /// <summary>The radius of the blob shadow</summary>
        public float ShadowRadius;

        /// <summary>The radius of the cylinder collider</summary>
        public float ColliderRadius;

        /// <summary>Unknown. Usually 0x0000.</summary>
        public ushort Unkx4C;

        /// <summary>The type of sprite FX</summary>
        public FX2DType FXType;

        /// <summary>The control point to root the sprite FX on</summary>
        public byte FXControlPointID;

        /// <summary>The frame length of the sprite FX</summary>
        public byte FXLength;

        /// <summary>Unknown. Usually 0x0000.</summary>
        public ushort Unkx52;

        /// <summary>The turning speed of the NPC</summary>
        public float TurnSpeed;

        /// <summary>Standing Talk Forward Frame Length</summary>
        public byte StandTalkForwardLength;

        /// <summary>Standing Talk Left+Right Frame Length</summary>
        public byte StandTalkLeftRightLength;

        /// <summary>Sitting Talk Forward Frame Length</summary>
        public byte SittingTalkForwardLength;

        /// <summary>Sitting Talk Left+Right Frame Length</summary>
        public byte SittingTalkLeftRightLength;

        /// <summary>If the NPC uses an external texture</summary>
        public bool UseExternalTexture;

        /// <summary>The filename of the external texture</summary>
        public string ExternalTextureFile;

        /// <summary>Unknown.</summary>
        public uint Unkx7C;

        public List<List<PrfSfxItem>> SpecialEffectsPerAnim;
        public List<List<PrfSndItem>> SoundsPerAnim;

        public static EditorNpcPRF Create()
        {
            EditorNpcPRF result = new EditorNpcPRF();

            result.Name                       = "New NPC";
            result.ModelFile                  = string.Empty;
            result.Unkx3E                     = 0x0100;
            result.ColliderHeight             = 1.76f;
            result.ShadowRadius               = 1.0f;
            result.ColliderRadius             = 0.8f;
            result.Unkx4C                     = 0x0000;
            result.FXType                     = FX2DType.None;
            result.FXControlPointID           = 0;
            result.FXLength                   = 0;
            result.Unkx52                     = 0x0000;
            result.TurnSpeed                  = 1.0f;
            result.StandTalkForwardLength     = 0;
            result.StandTalkLeftRightLength   = 0;
            result.SittingTalkForwardLength   = 0;
            result.SittingTalkLeftRightLength = 0;
            result.UseExternalTexture         = false;
            result.ExternalTextureFile        = string.Empty;
            result.Unkx7C                     = 0x00000000;

            result.SpecialEffectsPerAnim = new List<List<PrfSfxItem>>(32);
            for (int i = 0; i < 32; ++i)
                result.SpecialEffectsPerAnim.Add(new List<PrfSfxItem>());

            result.SoundsPerAnim = new List<List<PrfSndItem>>(32);
            for (int i = 0; i < 32; ++i)
                result.SoundsPerAnim.Add(new List<PrfSndItem>());

            return result;
        }

        public static EditorNpcPRF LoadFromFile(string filename)
        {
            EditorNpcPRF result = new EditorNpcPRF();

            using FileInputStream fIn = new FileInputStream(filename);

            result.Name = fIn.ReadFixedString(31, App.SJisEncoding);
            result.ModelFile = fIn.ReadFixedString(31, App.SJisEncoding);
            result.Unkx3E = fIn.ReadU16();
            result.ColliderHeight = fIn.ReadF32();
            result.ShadowRadius = fIn.ReadF32();
            result.ColliderRadius = fIn.ReadF32();
            result.Unkx4C = fIn.ReadU16();
            result.FXType = (FX2DType)fIn.ReadS16();
            result.FXControlPointID = fIn.ReadU8();
            result.FXLength = fIn.ReadU8();
            result.Unkx52 = fIn.ReadU16();
            result.TurnSpeed = fIn.ReadF32();
            result.StandTalkForwardLength = fIn.ReadU8();
            result.StandTalkLeftRightLength = fIn.ReadU8();
            result.SittingTalkForwardLength = fIn.ReadU8();
            result.SittingTalkLeftRightLength = fIn.ReadU8();
            result.UseExternalTexture = (fIn.ReadU8() == 1);
            result.ExternalTextureFile = fIn.ReadFixedString(31, App.SJisEncoding);
            result.Unkx7C = fIn.ReadU32();

            // Now are FX, SFX Entries. 0x100 bytes of this crap...

            // 32 Sets of SFX...
            result.SpecialEffectsPerAnim = new List<List<PrfSfxItem>>();
            for (int i = 0; i < 32; ++i)
            {
                ushort sfxItemCount  = fIn.ReadU16();
                ushort sfxItemOffset = fIn.ReadU16();

                // If the item count is 0, add an empty array...
                if (sfxItemCount == 0)
                    result.SpecialEffectsPerAnim.Add(new List<PrfSfxItem>());
                else
                {
                    // Jump to and read the Sfx Item
                    fIn.Jump(sfxItemOffset);

                    PrfSfxItem[] items = new PrfSfxItem[sfxItemCount];

                    // Read each Sfx Item
                    for (int j = 0; j < sfxItemCount; ++j)
                    {
                        items[j] = new PrfSfxItem
                        {
                            delay = fIn.ReadU8(),
                            controlPoint = fIn.ReadU8(),
                            id = fIn.ReadS16()
                        };
                    }

                    result.SpecialEffectsPerAnim.Add(items.ToList());

                    fIn.Return();
                }
            }

            // And 32 sets of Sounds
            result.SoundsPerAnim = new List<List<PrfSndItem>>();
            for (int i = 0; i < 32; ++i)
            {
                ushort sndItemCount  = fIn.ReadU16();
                ushort sndItemOffset = fIn.ReadU16();

                if (sndItemCount == 0)
                    result.SoundsPerAnim.Add(new List<PrfSndItem>());
                else
                {
                    fIn.Jump(sndItemOffset);

                    PrfSndItem[] items = new PrfSndItem[sndItemCount];

                    for (int j = 0; j < sndItemCount; ++j)
                    {
                        items[j] = new PrfSndItem
                        {
                            delay = fIn.ReadU8(),
                            pitch = fIn.ReadS8(),
                            id = fIn.ReadS16()
                        };
                    }

                    result.SoundsPerAnim.Add(items.ToList());

                    fIn.Return();
                }
            }

            return result;
        }

        public void Save(string filepath)
        {
            if (File.Exists(Path.Combine(filepath, $"{Name}.prf")))
                File.Delete(Path.Combine(filepath, $"{Name}.prf"));

            using BinaryWriter bw = new BinaryWriter(File.Open(Path.Combine(filepath, $"{Name}.prf"), FileMode.CreateNew));

            // Name...
            byte[] nameAsBytes = App.SJisEncoding.GetBytes(Name);
            Array.Resize(ref nameAsBytes, 31);
            bw.Write(nameAsBytes);

            // Model File
            byte[] modelAsBytes = App.SJisEncoding.GetBytes(ModelFile);
            Array.Resize(ref modelAsBytes, 31);
            bw.Write(modelAsBytes);

            // ...
            bw.Write(Unkx3E);
            bw.Write(ColliderHeight);
            bw.Write(ShadowRadius);
            bw.Write(ColliderRadius);
            bw.Write(Unkx4C);
            bw.Write((short)FXType);
            bw.Write(FXControlPointID);
            bw.Write(FXLength);
            bw.Write(Unkx52);
            bw.Write(TurnSpeed);
            bw.Write(StandTalkForwardLength);
            bw.Write(StandTalkLeftRightLength);
            bw.Write(SittingTalkForwardLength);
            bw.Write(SittingTalkLeftRightLength);
            bw.Write((byte)(UseExternalTexture ? 1 : 0));

            // External Texture
            byte[] textureAsBytes = App.SJisEncoding.GetBytes(ExternalTextureFile);
            Array.Resize(ref textureAsBytes, 31);
            bw.Write(textureAsBytes);

            // ...
            bw.Write(Unkx7C);

            long writeOffset = 0x180;
            long returnOffset = 0;

            // Effect Items Per Anim
            for (int i = 0; i < 32; ++i)
            {
                // Item List
                List<PrfSfxItem> items = SpecialEffectsPerAnim[i];

                // If there are items, write the count and offset... then write the items themselves...
                if (items.Count > 0)
                {
                    // Write the info...
                    bw.Write((ushort)items.Count);
                    bw.Write((ushort)writeOffset);

                    // Write the effects themselves...
                    returnOffset = bw.BaseStream.Position;
                    bw.Seek((int)writeOffset, SeekOrigin.Begin);

                    // Write each effect.
                    foreach (PrfSfxItem item in items)
                    {
                        bw.Write(item.delay);
                        bw.Write(item.controlPoint);
                        bw.Write(item.id);
                    }

                    // Go back to the return point...
                    writeOffset = bw.BaseStream.Position;
                    bw.Seek((int)returnOffset, SeekOrigin.Begin);
                } 
                else
                {
                    bw.Write((ushort)0x00);
                    bw.Write((ushort)0x00);
                }
            }

            // Sounds Items Per Anim
            for (int i = 0; i < 32; ++i)
            {
                // Item List
                List<PrfSndItem> items = SoundsPerAnim[i];

                // If there are items, write the count and offset... then write the items themselves...
                if (items.Count > 0)
                {
                    // Write the info...
                    bw.Write((ushort)items.Count);
                    bw.Write((ushort)writeOffset);

                    // Write the effects themselves...
                    returnOffset = bw.BaseStream.Position;
                    bw.Seek((int)writeOffset, SeekOrigin.Begin);

                    // Write each effect.
                    foreach (PrfSndItem item in items)
                    {
                        bw.Write(item.delay);
                        bw.Write(item.pitch);
                        bw.Write(item.id);
                    }

                    // Go back to the return point...
                    writeOffset = bw.BaseStream.Position;
                    bw.Seek((int)returnOffset, SeekOrigin.Begin);
                }
                else
                {
                    bw.Write((ushort)0x00);
                    bw.Write((ushort)0x00);
                }
            }
        }
    }
}