using System.IO;
using System;
using UnityEngine;
using System.Collections.Generic;

public class ResourceManager
{
    /// <summary>Base Game Path</summary>
    public static string GamePath      { get; private set; }

    /// <summary>Game Data Path</summary>
    public static string GameDataPath  { get; private set; }

    /// <summary>Game Param Path</summary>
    public static string GameParamPath { get; private set; }

    /// <summary>Overrides Path</summary>
    public static string OverridesPath { get; private set; }

    public static List<AssetBundle> OverrideBundles { get; private set; }
    public static Dictionary<string, OverrideAsset> OverrideAssets { get; private set; }

    public ResourceManager()
    {
        GamePath      = Path.Combine(Path.GetFullPath(Application.streamingAssetsPath), "Game");
        GameDataPath  = Path.Combine(GamePath, "DATA");
        GameParamPath = Path.Combine(GamePath, "PARAM");
        OverridesPath = Path.Combine(Path.GetFullPath(Application.streamingAssetsPath), "Overrides");
    }

    public void LoadOverrides()
    {
        if (!Directory.Exists(OverridesPath))
            throw new Exception("No Overrides Directory!");

        // First create our storage...
        OverrideBundles = new List<AssetBundle>();
        OverrideAssets  = new Dictionary<string, OverrideAsset>();

        // Get each file in the overrides path
        string[] overrideFiles = Directory.GetFiles(OverridesPath, "*.lrt", SearchOption.AllDirectories);

        // Load each override file
        foreach(string lrtFile in overrideFiles)
        {
            // Load the bundle, store it in our list
            AssetBundle bundle = AssetBundle.LoadFromFile(lrtFile);
            OverrideBundles.Add(bundle);

            // Get the name of every file inside the bundle
            string[] assetNames = bundle.GetAllAssetNames();

            foreach (string asset in assetNames)
            {
                // Get the true bare name of the asset without extension...
                string assetTrueName = Path.GetFileNameWithoutExtension(asset);

                // If this asset already exists, we're overriding another override... Warn the player.
                if (OverrideAssets.ContainsKey(assetTrueName))
                    Logger.Warn($"Conflicting Override! '{assetTrueName}' overriden!");

                OverrideAssets[assetTrueName] = new OverrideAsset
                {
                    bundleID    = OverrideBundles.Count - 1,
                    pathToAsset = asset
                };
            }
        }
    }

    public static bool OverrideExists(string name) =>
        OverrideAssets.ContainsKey(name);

    public static T OverrideLoad<T>(string name) where T : UnityEngine.Object =>
        OverrideBundles[OverrideAssets[name].bundleID].LoadAsset<T>(OverrideAssets[name].pathToAsset);
}

public struct OverrideAsset
{
    public int bundleID;
    public string pathToAsset;
}