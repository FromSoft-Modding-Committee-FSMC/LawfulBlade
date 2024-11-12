using Lawful.IO;
using System;
using System.IO;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "GameInformation", menuName = "Lawful Runtime/Game Information")]
public class GameInformation : ScriptableObject
{
    [field: Header("Sequences")]
    [field: SerializeField] public GameSequenceInfo attractSequence { get; private set; }
    [field: SerializeField] public GameSequenceInfo openingSequence { get; private set; }
    [field: SerializeField] public GameSequenceInfo endingSequence1 { get; private set; }
    [field: SerializeField] public GameSequenceInfo endingSequence2 { get; private set; }
    [field: SerializeField] public GameSequenceInfo endingSequence3 { get; private set; }
    [field: SerializeField] public GameSequenceInfo creditsSequence { get; private set; }

    [field: Header("Images")]
    [field: SerializeField] public string titleImage { get; private set; }
    [field: SerializeField] public string closeImage { get; private set; }

    /// <summary>
    /// Imports legacy sys.dat file
    /// </summary>
    /// <param name="path">Path to SOM game sys.dat</param>
    public void ImportFromLegacyFile(string path)
    {
        //
        // WARNING:
        //  Technically all strings are SHIFT-JIS, but we'll cross that bridge when we come to it...
        //

        // Open the legacy file
        using FileInputStream ins = new(File.OpenRead(path));

        //
        // Sequences, Title & Close Image
        //
        attractSequence = LoadSequence(ins, "TITLE.DAT");
        titleImage      = ins.ReadFixedString(31);
        openingSequence = LoadSequence(ins, "OPENNING.DAT");
        endingSequence1 = LoadSequence(ins, "ENDING1.DAT");
        endingSequence2 = LoadSequence(ins, "ENDING2.DAT");
        endingSequence3 = LoadSequence(ins, "ENDING3.DAT");
        creditsSequence = LoadSequence(ins, "STAFF.DAT");
        closeImage      = ins.ReadFixedString(31);
    }

    /// <summary>
    /// Loads sequence information from the SYS.DAT file
    /// </summary>
    /// <param name="br">A BinaryReader to read from</param>
    /// <param name="slideshowSource">The source of the slide show if this is not a movie or null</param>
    /// <returns></returns>
    GameSequenceInfo LoadSequence(FileInputStream ins, string slideshowSource)
    {
        GameSequenceInfo temp = new()
        {
            mode = (SequenceMode)ins.ReadU8(),
            file = ins.ReadFixedString(31)
        };

        // Override with slideshow file if mode is for a slideshow
        switch(temp.mode)
        {
            case SequenceMode.Video:
                temp.file = Path.Combine(ResourceManager.GameDataPath, "MOVIE", temp.file);
                break;

            case SequenceMode.Slideshow:
                temp.file = Path.Combine(ResourceManager.GameParamPath, slideshowSource);
                break;
        }

        return temp;
    }
}

[Serializable]
public struct GameSequenceInfo
{
    public SequenceMode mode;
    public string file;
}