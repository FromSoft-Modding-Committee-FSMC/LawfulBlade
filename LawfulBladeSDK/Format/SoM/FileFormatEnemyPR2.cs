using LawfulBladeSDK.Extensions;
using LawfulBladeSDK.IO;

namespace LawfulBladeSDK.Format.SoM
{
    public class FileFormatEnemyPR2
    {
        public List<EnemyPRF> Profiles;
        public int ProfileCount => Profiles.Count;

        /// <summary>
        /// Loads a Enemy PR2 from a file.
        /// </summary>
        public static bool LoadFromFile(string filepath, out FileFormatEnemyPR2 pr2)
        {
            try
            {
                using FileInputStream fis = new FileInputStream(filepath);
                pr2 = Load(fis);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load Enemy PR2 '{Path.GetFileName(filepath)}': {ex.Message}".Colourise(0xF08080));
            }

            pr2 = null;
            return false;
        }

        /// <summary>
        /// Internal loading for Enemy PR2 format.
        /// </summary>
        static FileFormatEnemyPR2 Load(FileInputStream fis)
        {
            FileFormatEnemyPR2 result = new FileFormatEnemyPR2();

            // Load each PRF from the stream
            result.Profiles = new List<EnemyPRF>(1024);

            for (int i = 0; i < 1024; ++i)
            {
                // Read an offset to PRF data
                uint prfOffset = fis.ReadU32();

                // Skip empty entries
                if (prfOffset == 0)
                    continue;

                fis.Jump(prfOffset);
                result.Profiles.Add(fis.ReadStruct<EnemyPRF>());
                fis.Return();
            }
                
            return result;
        }
    }
}
