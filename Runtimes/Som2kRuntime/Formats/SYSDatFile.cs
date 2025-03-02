using LawfulBladeSDK.IO;
using System.Text;

namespace Som2kRuntime.Formats
{
    internal class SYSDatFile
    {
        public readonly int    OpenSequenceMode;    // 0 = skip, 1 = video, 2 = slideshow
        public readonly string OpenSequenceFile;    // When mode is video, this is name of the video file

        public readonly int    IntroSequenceMode;   // See 'OpenSequenceMode'
        public readonly string IntroSequenceFile;   // See 'OpenSequenceFile'

        public readonly int    End1SequenceMode;    // See 'OpenSequenceMode'
        public readonly string End1SequenceFile;    // See 'OpenSequenceFile'

        public readonly int    End2SequenceMode;    // See 'OpenSequenceMode'
        public readonly string End2SequenceFile;    // See 'OpenSequenceFile'

        public readonly int    End3SequenceMode;    // See 'OpenSequenceMode'
        public readonly string End3SequenceFile;    // See 'OpenSequenceFile'

        public readonly int    StaffSequenceMode;   // See 'OpenSequenceMode'
        public readonly string StaffSequenceFile;   // See 'OpenSequenceFile'

        public readonly string TitleImageFile;
        public readonly string FinalImageFile;

        public SYSDatFile(string filename)
        {
            // Get S-JIS encoding
            Encoding SJISEnc = Encoding.GetEncoding(932);

            using FileInputStream fis = new FileInputStream(filename);

            // Read Sequence Info...
            OpenSequenceMode  = fis.ReadU8();
            OpenSequenceFile  = fis.ReadFixedString(31, SJISEnc);
            TitleImageFile    = fis.ReadFixedString(31, SJISEnc);
            IntroSequenceMode = fis.ReadU8();
            IntroSequenceFile = fis.ReadFixedString(31, SJISEnc);
            End1SequenceMode  = fis.ReadU8();
            End1SequenceFile  = fis.ReadFixedString(31, SJISEnc);
            End2SequenceMode  = fis.ReadU8();
            End2SequenceFile  = fis.ReadFixedString(31, SJISEnc);
            End3SequenceMode  = fis.ReadU8();
            End3SequenceFile  = fis.ReadFixedString(31, SJISEnc);
            StaffSequenceMode = fis.ReadU8();
            StaffSequenceFile = fis.ReadFixedString(31, SJISEnc);
            FinalImageFile    = fis.ReadFixedString(31, SJISEnc);
        }
    }
}