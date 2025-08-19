using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateAssetBundles
{
    [MenuItem ("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles ()
    {
        string assetBundleBuildPath = $"{Application.streamingAssetsPath}/{ABMConstants.BundlePath}";
        if (!Directory.Exists(assetBundleBuildPath))
        {
            Directory.CreateDirectory(assetBundleBuildPath);
        }
        BuildPipeline.BuildAssetBundles (assetBundleBuildPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }
}