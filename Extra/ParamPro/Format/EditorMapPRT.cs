using LawfulBladeSDK.IO;
using System.Text;

namespace ParamPro.Format
{
    public class EditorMapPRT
    {
        /// <summary>The name of the MSM file used for rendering</summary>
        public string MSMFileName { get; set; } = string.Empty;

        /// <summary>The name of the MHM file used for collision</summary>
        public string MHMFileName { get; set; } = string.Empty;

        /// <summary>The cardinal directions the tile occludes</summary>
        public byte OccludingCardinals { get; set; } = 0;

        /// <summary>The ID of the icon used for automapping.</summary>
        public byte AutomapIconID { get; set; } = 0;

        /// <summary>BIT0 = Poison Trap, BIT1 = Pit Trap</summary>
        public byte Flags { get; set; } = 0;

        /// <summary>The name of the BMP file used for an editor icon.</summary>
        public string EditorIconFileName { get; set; } = string.Empty;

        /// <summary>The name of the PRT.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Loads a PRT from a file
        /// </summary>
        public static EditorMapPRT LoadFromFile(string filename)
        {
            // Open the file as an input stream and read its data
            using FileInputStream fIn = new FileInputStream(filename);

            // Create returned object
            EditorMapPRT result = new EditorMapPRT();

            // Read initial data
            result.MSMFileName = fIn.ReadFixedString(31, App.SJisEncoding);
            fIn.SeekRelative(1);    // Padding
            result.MHMFileName = fIn.ReadFixedString(31, App.SJisEncoding);
            fIn.SeekRelative(1);    // Padding
            result.OccludingCardinals = fIn.ReadU8();
            result.AutomapIconID      = fIn.ReadU8();
            result.Flags              = fIn.ReadU8();
            fIn.SeekRelative(1);    // Padding ?
            result.EditorIconFileName = fIn.ReadFixedString(31, App.SJisEncoding);
            fIn.SeekRelative(1);    // Padding
            result.Name = fIn.ReadFixedString(128, App.SJisEncoding);   // Seriously long name field.

            return result;
        }

        /// <summary>
        /// Saves a PRT to a file
        /// </summary>
        /// <param name="filename"></param>
        public void SaveToFile(string filename)
        {

        }
    }
}
