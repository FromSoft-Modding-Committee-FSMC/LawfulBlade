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

    [Header("Debugging")]
    [SerializeField] bool loadInEditor;

    /// <summary>
    /// ScriptableObject Implementation.<br/>
    /// Used for load in editor functionality.
    /// </summary>
    void OnValidate()
    {
        if (loadInEditor)
        {
            // Construct the default path to sys.dat
            string filePath = Path.Combine(Path.GetFullPath(Application.streamingAssetsPath), "GAME", "PARAM", "sys.dat");

            // Attempt to import from the legacy file
            ImportFromLegacyFile(filePath);

            // Re-disable load in editor...
            loadInEditor = false;
        }
    }

    /// <summary>
    /// Imports legacy sys.dat file
    /// </summary>
    /// <param name="path">Path to SOM game sys.dat</param>
    public void ImportFromLegacyFile(string path)
    {
        // Open the legacy file
        using (BinaryReader br = new (File.OpenRead(path)))
        {
            // Read attract sequence
            attractSequence = LoadSequence(br, "TITLE.DAT");

            // Read title image
            titleImage = Sanitize(Encoding.ASCII.GetString(br.ReadBytes(31)));

            // Read opening Sequence
            openingSequence = LoadSequence(br, "OPENNING.DAT");

            // Read ending sequences
            endingSequence1 = new GameSequenceInfo
            {
                mode = (SequenceMode)br.ReadByte(),
                file = Encoding.ASCII.GetString(br.ReadBytes(31)).Trim('\0')
            };

            endingSequence2 = new GameSequenceInfo
            {
                mode = (SequenceMode)br.ReadByte(),
                file = Encoding.ASCII.GetString(br.ReadBytes(31))
            };

            endingSequence3 = new GameSequenceInfo
            {
                mode = (SequenceMode)br.ReadByte(),
                file = Encoding.ASCII.GetString(br.ReadBytes(31))
            };

            // Read credits sequence
            creditsSequence = new GameSequenceInfo
            {
                mode = (SequenceMode)br.ReadByte(),
                file = Encoding.ASCII.GetString(br.ReadBytes(31))
            };

            // Read close image
            closeImage = Encoding.ASCII.GetString(br.ReadBytes(31));
        }
    }

    string Sanitize(string f) =>
        f[..f.IndexOf('\0')].Trim();

    GameSequenceInfo LoadSequence(BinaryReader br, string slideshowSource)
    {
        GameSequenceInfo temp = new()
        {
            mode = (SequenceMode)br.ReadByte(),
            file = Encoding.ASCII.GetString(br.ReadBytes(31))
        };

        // Override with slideshow file if mode is for a slideshow
        switch(temp.mode)
        {
            case SequenceMode.Video:
                Logger.Warn(string.Join(", ", Encoding.ASCII.GetBytes(Sanitize(temp.file))));
                temp.file = Path.Combine(Path.GetFullPath(Application.streamingAssetsPath), "Game", "DATA", "MOVIE", Sanitize(temp.file));
                break;

            case SequenceMode.Slideshow:
                temp.file = Path.Combine(Path.GetFullPath(Application.streamingAssetsPath), "Game", "PARAM", slideshowSource);
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