using LawfulBladeSDK.Extensions;
using LawfulBladeSDK.IO;

namespace LawfulBladeSDK.Format.SoM
{
    public class FileFormatNpcPR2
    {
        public List<NpcPRF> Profiles;
        public int ProfileCount => Profiles.Count;

        /// <summary>
        /// Loads a NPC PR2 from a file.
        /// </summary>
        public static bool LoadFromFile(string filepath, out FileFormatNpcPR2 pr2)
        {
            try
            {
                using FileInputStream fis = new FileInputStream(filepath);
                pr2 = Load(fis);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load NPC PR2 '{Path.GetFileName(filepath)}': {ex.Message}".Colourise(0xF08080));
            }

            pr2 = null;
            return false;
        }

        /// <summary>
        /// Internal loading for NPC PR2 format.
        /// </summary>
        static FileFormatNpcPR2 Load(FileInputStream fis)
        {
            FileFormatNpcPR2 result = new FileFormatNpcPR2();

            // Load each PRF from the stream
            result.Profiles = new List<NpcPRF>(1024);

            for (int i = 0; i < 1024; ++i)
            {
                // Read an offset to PRF data
                uint prfOffset = fis.ReadU32();

                // Skip empty entries
                if (prfOffset == 0)
                    continue;

                fis.Jump(prfOffset);
                result.Profiles.Add(fis.ReadStruct<NpcPRF>());
                fis.Return();
            }

            return result;
        }
    }
}
