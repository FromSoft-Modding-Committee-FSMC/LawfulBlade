using LawfulBladeSDK.Extensions;
using LawfulBladeSDK.IO;

namespace LawfulBladeSDK.Format.SoM
{
    public class FileFormatObjectPR2
    {
        public ObjectPRF[] Profiles { get; private set; }
        public int ProfileCount => Profiles.Length;

        /// <summary>
        /// Loads a Object PR2 from a file.
        /// </summary>
        public static bool LoadFromFile(string filepath, out FileFormatObjectPR2 pr2)
        {
            try
            {
                using FileInputStream fis = new FileInputStream(filepath);
                pr2 = Load(fis);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load Object PR2 '{Path.GetFileName(filepath)}': {ex.Message}".Colourise(0xF08080));
            }

            pr2 = null;
            return false;
        }

        /// <summary>
        /// Internal loading for Object PR2 format.
        /// </summary>
        static FileFormatObjectPR2 Load(FileInputStream fis)
        {
            FileFormatObjectPR2 result = new FileFormatObjectPR2();

            // Load each PRF from the stream
            int pr2ProfileNum = fis.ReadS32();

            result.Profiles = new ObjectPRF[pr2ProfileNum];

            for (int i = 0; i < pr2ProfileNum; ++i)
            {
                result.Profiles[i] = new ObjectPRF
                {
                    name      = fis.ReadFixedString(31, EncodingExtensions.SJISEncoding),
                    modelFile = fis.ReadFixedString(31, EncodingExtensions.SJISEncoding),
                    billboard = fis.ReadU8() == 1,
                    openable  = fis.ReadU8() == 1,
                    colliderHeight = fis.ReadF32(),
                    colliderRW = fis.ReadF32(),
                    colliderRD = fis.ReadF32(),
                    f32x4C = fis.ReadF32(),
                    colliderMode = fis.ReadU8(),
                    hasScrollingTexture1 = fis.ReadU8() == 1,
                    objectClass = fis.ReadS16(),
                    effectID = fis.ReadS16(),
                    effectControlPointAnchor = fis.ReadU8(),
                    effectAnimationRate = fis.ReadU8(),
                    loopingSoundFxID = fis.ReadS16(),
                    openingSoundFxID = fis.ReadS16(),
                    closingSoundFxID = fis.ReadS16(),
                    loopingSoundFxDelay = fis.ReadU8(),
                    openingSoundFxDelay = fis.ReadU8(),
                    closingSoundFxDelay = fis.ReadU8(),
                    loopingSoundFxPitch = fis.ReadS8(),
                    openingSoundFxPitch = fis.ReadS8(),
                    closingSoundFxPitch = fis.ReadS8(),
                    trapEffectID = fis.ReadS16(),
                    trapEffectOrientate = fis.ReadU8() == 1,
                    trapEffectVisible = fis.ReadU8() == 1,
                    loopAnimation = fis.ReadU8() == 1,
                    invisible = fis.ReadU8() == 1,
                    slotKeyID = fis.ReadU8(),
                    allowXZRotation = fis.ReadU8() == 1
                };
            }

            return result;
        }
    }
}
