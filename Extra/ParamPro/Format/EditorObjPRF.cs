using LawfulBladeSDK.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParamPro.Format
{
    public class EditorObjPRF
    {
        public enum EditorObjFX : short
        {
            None         = -1,
            Flame1       = 0x000B,
            Flame2       = 0x000C,
            WallOfLight1 = 0x0032,
            WallOfLight2 = 0x0033,
            Splash       = 0x0034,
            WallOfLight3 = 0x0037,
            WallOfLight4 = 0x0038,
            Flame3       = 0x0054,
            Flame4       = 0x0055,
            Flame5       = 0X01C4
        }

        public enum EditorObjContoller : ushort
        {
            Ornament     = 0x00,    // The object does fuck all
            Lamp         = 0x0A,    // The object emits light
            SlidingDoor  = 0x0B,    // The object is a door which slides open
            SwingingDoor = 0x0D,    // The object is a door which swings open
            DoubleDoor   = 0x0E,    // The object is two doors which swing open
            Container    = 0x14,    // The object is a container (crate, barrel)
            Chest        = 0x15,    // The object is a chest
            Corpse       = 0x16,    // The object is a corpse
            MeleeTrap    = 0x1E,    // The object is a trap which requires you be near it.
            RangedTrap   = 0x1F,    // The object is a trap which produces a projectile
            Switch       = 0x28,    // The object is a switch
            Slot         = 0x29     // The object acts as a slot for an item
        }

        public enum EditorObjCollider : byte
        {
            Cylinder,
            Cuboid,
            Unknown
        }

        public struct EditorSoundFX
        {
            public short soundID;
            public byte  frameDelay;
            public sbyte pitch;
        }

        /// <summary>
        /// The name of the profile
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The name of the MDO/MDL file
        /// </summary>
        public string ModelFileName { get; set; }

        /// <summary>
        /// Set if the object is a billboard 
        /// </summary>
        public bool IsBillboard { get; set; }

        /// <summary>
        /// Set if the object is openable
        /// </summary>
        public bool IsOpenable { get; set; }

        /// <summary>
        /// The height of collision object
        /// </summary>
        public float ColliderHeight { get; set; }

        /// <summary>
        /// The width or radius of the collision object
        /// </summary>
        public float ColliderRadiusWidth { get; set; }

        /// <summary>
        /// The depth of the collision object
        /// </summary>
        public float ColliderDepth { get; set; }

        /// <summary>
        /// Unknown. Something to do with traps?
        /// </summary>
        public float f32x4c { get; set; }

        /// <summary>
        /// The shape of the collider
        /// </summary>
        public EditorObjCollider ColliderShape { get; set; } // 0 = Cylinder, 1 = Cuboid, 2 = Special (???)

        /// <summary>
        /// Texture scroll mode
        /// </summary>
        public byte TextureScrollMode { get; set; } // 0 = Don't Scroll Texture, 1 = Scroll U, 2 = Scroll V

        /// <summary>
        /// The controller class ID
        /// </summary>
        public EditorObjContoller ClassID { get; set; }

        /// <summary>
        /// 2D FX: id of the FX played (-1 is none)
        /// </summary>
        public EditorObjFX FxType { get; set; }

        /// <summary>
        /// 2D FX: target control point (where the fx origin is starts)
        /// </summary>
        public byte FxControlPoint { get; set; }

        /// <summary>
        /// 2D FX: number of frames before looping
        /// </summary>
        public byte FxFrames { get; set; }

        public EditorSoundFX loopingSound;
        public EditorSoundFX openingSound;
        public EditorSoundFX closingSound;

        public short TrapSfxID { get; set; }
        public byte TrapSfxDirectional { get; set; }
        public byte TrapSfxVisible { get; set; }

        public bool LoopAnimation { get; set; }
        public bool Visible { get; set; }           // Actually invisible to SoM, we're inverting it because it makes more sense to toggle visibility
        public byte SlotKey { get; set; }           // a slot 'key', which is not a key as in an item but a code shared between the item and the object.
        public bool FreeRotation { get; set; }      // If rotation on X and Z is allowed.

        /// <summary>
        /// Loads a PRF from a file
        /// </summary>
        public static EditorObjPRF LoadFromFile(string filename)
        {
            using FileInputStream fIn = new FileInputStream(filename);

            EditorObjPRF result = new EditorObjPRF();

            // Read all information we can...

            // 0x00
            result.Name          = fIn.ReadFixedString(31, App.SJisEncoding);
            result.ModelFileName = fIn.ReadFixedString(31, App.SJisEncoding);
            result.IsBillboard   = fIn.ReadU8() != 0;
            result.IsOpenable    = fIn.ReadU8() != 0;

            // 0x40
            result.ColliderHeight = fIn.ReadF32();
            result.ColliderRadiusWidth = fIn.ReadF32();
            result.ColliderDepth = fIn.ReadF32();
            result.f32x4c = fIn.ReadF32();

            // 0x50
            result.ColliderShape = (EditorObjCollider)fIn.ReadU8();
            result.TextureScrollMode = fIn.ReadU8();
            result.ClassID = (EditorObjContoller)fIn.ReadU16();
            result.FxType = (EditorObjFX)fIn.ReadS16();
            result.FxControlPoint = fIn.ReadU8();
            result.FxFrames = fIn.ReadU8();

            // 0x58
            result.loopingSound.soundID = fIn.ReadS16();
            result.openingSound.soundID = fIn.ReadS16();
            result.closingSound.soundID = fIn.ReadS16();
            result.loopingSound.frameDelay = fIn.ReadU8();
            result.openingSound.frameDelay = fIn.ReadU8();
            result.closingSound.frameDelay = fIn.ReadU8();
            result.loopingSound.pitch = fIn.ReadS8();
            result.openingSound.pitch = fIn.ReadS8();
            result.closingSound.pitch = fIn.ReadS8();

            // 0x64
            result.TrapSfxID = fIn.ReadS16();
            result.TrapSfxDirectional = fIn.ReadU8();
            result.TrapSfxVisible = fIn.ReadU8();

            // 0x68
            result.LoopAnimation = (fIn.ReadU8() == 1);
            result.Visible       = (fIn.ReadU8() == 0);
            result.SlotKey       =  fIn.ReadU8();
            result.FreeRotation  = (fIn.ReadU8() == 1);

            return result;
        }
    }
}
