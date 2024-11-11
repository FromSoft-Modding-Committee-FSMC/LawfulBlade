using System.IO;
using UnityEngine;

public class ResourceManager
{
    public static string GamePath      { get; private set; }
    public static string GameDataPath  { get; private set; }
    public static string GameParamPath { get; private set; }

    static ResourceManager()
    {
        GamePath      = Path.Combine(Path.GetFullPath(Application.streamingAssetsPath), "Game");
        GameDataPath  = Path.Combine(GamePath, "DATA");
        GameParamPath = Path.Combine(GamePath, "PARAM");
    }
}
