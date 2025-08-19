using System.IO;
using UnityEditor;
using UnityEngine;

namespace Scripts.Core.AssetBundleManager.Editor
{
    public class CreateAssetBundles
    {
        [MenuItem("Assets/Build AssetBundles")]
        private static void BuildAllAssetBundles()
        {
            string assetBundleBuildPath = $"{Application.streamingAssetsPath}/{ABMConstants.BundlePath}";
            
            if (!Directory.Exists(assetBundleBuildPath))
            {
                Directory.CreateDirectory(assetBundleBuildPath);
            }
            
            BuildPipeline.BuildAssetBundles(assetBundleBuildPath, BuildAssetBundleOptions.None,
                EditorUserBuildSettings.activeBuildTarget);
        }
    }
}