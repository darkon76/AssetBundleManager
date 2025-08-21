using System.IO;
using UnityEditor;
using UnityEngine;

namespace Scripts.Core.AssetBundleManager.Editor
{
    [InitializeOnLoad]
    static class AutoBuildAssets
    {
        //Because the test requires that the reviewer open unity, press play and see a sample prefab loaded from an asset bundle. 
        //For a smoother experience we build the asset bundles at the initialize on load of the editor.
        //In a normal project it is recommended that the CI or manually create the bundles. 
        static AutoBuildAssets()
        {
            if (!SessionState.GetBool("BundlesBuild", false))
            {
                // Startup code here...
                CreateAssetBundles.BuildAllAssetBundles();
                SessionState.SetBool("BundlesBuild", true);
            }
        }
    }
    
    public class CreateAssetBundles
    {
        [MenuItem("Assets/Build AssetBundles")]
        public static void BuildAllAssetBundles()
        {
            //We need to be sure that everything is linked correctly before building. 
            if (!AssetValidator.ValidateAssets())
            {
                Debug.LogError("BuildAllAssetBundles interrupted validateAssets failed");
                return;
            }
            
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