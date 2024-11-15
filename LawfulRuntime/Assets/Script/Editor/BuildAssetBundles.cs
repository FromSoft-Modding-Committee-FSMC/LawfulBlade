using UnityEngine;
using UnityEditor;
using System.IO;

public static class BuildAssetBundles
{
    static readonly string AssetBundlePath          = Path.Combine($"{Application.dataPath}/../", "Assets", "AssetBundles");
    const BuildAssetBundleOptions AssetBuildOptions = BuildAssetBundleOptions.AssetBundleStripUnityVersion | BuildAssetBundleOptions.ChunkBasedCompression;

    [MenuItem("Asset Bundles/Build")]
    public static void BuildBundles()
    {
        // Make sure the AssetBundle directory exists before we begin...
        if (!Directory.Exists(AssetBundlePath))
            Directory.CreateDirectory(AssetBundlePath);

        // Actually build the asset bundles
        BuildPipeline.BuildAssetBundles(AssetBundlePath, AssetBuildOptions, BuildTarget.StandaloneWindows64);
    }
}
