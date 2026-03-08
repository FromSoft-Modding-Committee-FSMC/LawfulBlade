using LawfulBladeSDK.Extensions;
using LawfulBladeSDK.IO;

namespace LawfulBladeSDK.Format.SoM
{
    public class FileFormatMDO
    {
        public string[] Textures { get; private set; }

        /// <summary>
        /// Loads an MDO from a file.
        /// </summary>
        public static bool LoadFromFile(string filepath, out FileFormatMDO mdo)
        {
            try
            {
                using FileInputStream fis = new FileInputStream(filepath);
                mdo = Load(fis);
                return true;
            } 
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load MDO '{Path.GetFileName(filepath)}': {ex.Message}".Colourise(0xF08080));
            }

            mdo = null;
            return false;
        }

        /// <summary>
        /// Internal loading for MDO format.<br/>Accepts FileInputStream so source can be either a file or memory path
        /// </summary>
        static FileFormatMDO Load(FileInputStream fis)
        {
            FileFormatMDO result = new FileFormatMDO();

            // Texture Data...
            int mdoTextureNum = fis.ReadS32();
            result.Textures = new string[mdoTextureNum];

            for (int i = 0; i < mdoTextureNum; ++i)
                // MDO says the textures are BMP, but they're really TXR
                result.Textures[i] = Path.ChangeExtension(fis.ReadTerminatedString(EncodingExtensions.SJISEncoding), "txr"); ;
            
            fis.Align(4);

            // Material Data...

            // TO-DO. We only care about textures currently...


            return result;
        }
    }
}
