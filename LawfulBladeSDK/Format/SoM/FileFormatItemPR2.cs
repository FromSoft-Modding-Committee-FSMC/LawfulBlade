using LawfulBladeSDK.Extensions;
using LawfulBladeSDK.IO;

namespace LawfulBladeSDK.Format.SoM
{
    public class FileFormatItemPR2
    {
        public List<ItemPRF> Profiles;
        public int ProfileCount => Profiles.Count;

        /// <summary>
        /// Loads a Item PR2 from a file.
        /// </summary>
        public static bool LoadFromFile(string filepath, out FileFormatItemPR2 pr2)
        {
            try
            {
                using FileInputStream fis = new FileInputStream(filepath);
                pr2 = Load(fis);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load Item PR2 '{Path.GetFileName(filepath)}': {ex.Message}".Colourise(0xF08080));
            }

            pr2 = null;
            return false;
        }

        /// <summary>
        /// Saves the content of the PR2 to a buffer
        /// </summary>
        public bool SaveToBuffer(out byte[] buffer)
        {
            using MemoryStream ms = new MemoryStream();
            using FileOutputStream fos = new FileOutputStream(ms);

            fos.WriteS32(Profiles.Count);

            foreach (ItemPRF prf in Profiles)
                fos.WriteStruct(prf);

            buffer = ms.ToArray();
            return true;
        }

        /// <summary>
        /// Internal loading for Item PR2 format.
        /// </summary>
        static FileFormatItemPR2 Load(FileInputStream fis)
        {
            FileFormatItemPR2 result = new FileFormatItemPR2();

            // Load each PRF from the stream
            int pr2ProfileNum = fis.ReadS32();

            result.Profiles = new List<ItemPRF>(pr2ProfileNum);

            for (int i = 0; i < pr2ProfileNum; ++i)
                result.Profiles.Add(fis.ReadStruct<ItemPRF>());

            return result;
        }
    }
}