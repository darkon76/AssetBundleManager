using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Core.AssetBundleManager
{
    public class AssetLoader
    {
        //The test request just spawning objects that is why a simple dictionary is enough. 
        //A proper solution will be a more robust system that counts references so we can dynamically unload bundles when needed. 
        private readonly Dictionary<string, AssetBundle> _loadedBundles = new();

        //Normally I use code injection but it adds a lot of overhead and noise to the test so a simple singleton is enough. 
        public static AssetLoader Instance { get; } = new();

        public T Instantiate<T>(AssetReference reference, Transform parent = null) where T : Object
        {
            var assetRef = GetAsset<T>(reference);
            if (assetRef == null)
            {
                return null;
            }
            return Object.Instantiate(assetRef, parent);
        }

        public T GetAsset<T>(AssetReference reference) where T : Object
        {
            if (!_loadedBundles.TryGetValue(reference.Bundle, out var bundle))
            {
                bundle = LoadBundle(reference);
                if (bundle == null)
                {
                    Debug.LogError($"{nameof(AssetLoader)} - Instantiate bundle wasn't found: {reference.Bundle}");
                    return null;
                }

                _loadedBundles.Add(reference.Bundle, bundle);
            }

            return bundle.LoadAsset<T>(reference.Name);
        }

        private AssetBundle LoadBundle(AssetReference assetReference)
        {
            //To keep it simple it uses hardcoded paths and synchronous loading. 
            string assetBundleBuildPath = $"{Application.streamingAssetsPath}/{ABMConstants.BundlePath}";
            return AssetBundle.LoadFromFile($"{assetBundleBuildPath}/{assetReference.Bundle}");
        }
    }
}