

using LawfulBladeSDK.Extensions;
using LawfulBladeSDK.IO;

namespace LawfulBladeSDK.Format.SoM
{
    public class FileFormatSysDAT
    {
        public SysDatSequence titleSequence;
        public SysDatSequence openingSequence;
        public SysDatSequence gameEnd1Sequence;
        public SysDatSequence gameEnd2Sequence;
        public SysDatSequence gameEnd3Sequence;
        public SysDatSequence staffSequence;
        public string titleBackground;
        public string closeBackground;

        public byte enableDashing;
        public byte unkxFF;             // Padding ?

        public SysDatPlayerSettingA playerSettingA;

        public SysDatClassSetting classData;

        public SysDatMagicSetting magicSetting;

        public SysDatMenuSetting menuSetting;

        public static bool LoadFromFile(string filepath, out FileFormatSysDAT sysDat)
        {
            try
            {
                using FileInputStream fis = new FileInputStream(filepath);
                sysDat = Load(fis);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load MDO '{Path.GetFileName(filepath)}': {ex.Message}".Colourise(0xF08080));
            }

            sysDat = null;
            return false;
        }

        static FileFormatSysDAT Load(FileInputStream fis)
        {
            FileFormatSysDAT result = new FileFormatSysDAT();

            // Sequence + Image Data
            result.titleSequence    = fis.ReadStruct<SysDatSequence>();
            result.titleBackground  = fis.ReadFixedString(31, EncodingExtensions.SJISEncoding);
            result.openingSequence  = fis.ReadStruct<SysDatSequence>();
            result.gameEnd1Sequence = fis.ReadStruct<SysDatSequence>();
            result.gameEnd2Sequence = fis.ReadStruct<SysDatSequence>();
            result.gameEnd3Sequence = fis.ReadStruct<SysDatSequence>();
            result.staffSequence    = fis.ReadStruct<SysDatSequence>();
            result.closeBackground  = fis.ReadFixedString(31, EncodingExtensions.SJISEncoding);

            // Misc #1
            result.enableDashing = fis.ReadU8();
            result.unkxFF        = fis.ReadU8();

            // Player Setting A
            result.playerSettingA = fis.ReadStruct<SysDatPlayerSettingA>();

            // Class Data
            result.classData = fis.ReadStruct<SysDatClassSetting>();

            // Magic Data
            result.magicSetting = fis.ReadStruct<SysDatMagicSetting>();

            // Menu Setting
            result.menuSetting = fis.ReadStruct<SysDatMenuSetting>();

            //
            // REST TO-DO
            //

            return result;
        }
    }
}