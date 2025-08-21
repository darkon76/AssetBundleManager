using System.Reflection;
using Scripts.Core.AssetBundleManager;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AssetValidator
{
    private static BindingFlags MonoValidationFieldFlag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
    [MenuItem("Assets/Validate Assets")]
    public static bool ValidateAssets()
    {
        bool isValid = true;
        isValid &= ValidateScenes();
        isValid &= ValidatePrefabs();
        return isValid;
    }

    public static bool ValidateScenes()
    {
        bool isValid = true;
        var guids = AssetDatabase.FindAssets("t:scene");
        Scene activeScene = SceneManager.GetActiveScene();
        foreach (var guid in guids)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            if (assetPath.StartsWith("Packages"))
            {
                continue;
            }
            var asset = AssetDatabase.LoadMainAssetAtPath(assetPath);
            if (!(asset is SceneAsset scene))
            {
                continue;
            }

            Scene loadedScene = EditorSceneManager.OpenScene(assetPath, OpenSceneMode.Additive);

            var rootGameObjects = loadedScene.GetRootGameObjects();
            foreach (var rootGameObject in rootGameObjects)
            {
                //We can do recursive search for the children.
                Transform[] childrenTransform = rootGameObject.GetComponentsInChildren<Transform>();
                foreach (var child in childrenTransform)
                {
                    isValid &= ValidateGameObject(child.gameObject);
                }
            }

            EditorSceneManager.SaveScene(loadedScene, assetPath);
        }

        EditorSceneManager.SetActiveScene(activeScene);
        return isValid;
    }
    
    public static bool ValidatePrefabs()
    {
        bool isValid = true;
        var guids = AssetDatabase.FindAssets("t:prefab");
        foreach (var guid in guids)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            if (assetPath.StartsWith("Packages"))
            {
                continue;
            }
            var asset = AssetDatabase.LoadMainAssetAtPath(assetPath);
            if (!(asset is GameObject go))
            {
                continue;
            }
            isValid &= ValidateGameObject(go);
        }

        return isValid;
    }

    private static bool ValidateGameObject(GameObject go)
    {
        bool isValid = true;
        var monos = go.GetComponentsInChildren<MonoBehaviour>();
        foreach (var mono in monos)
        {
            var type = mono.GetType();
            var fields = type.GetFields(MonoValidationFieldFlag);
            foreach (var field in fields)
            {
                if (typeof(AssetReference).IsAssignableFrom(field.FieldType))
                {
                    var assetReference = field.GetValue(mono) as AssetReference;
                    var assetPath = AssetDatabase.GUIDToAssetPath(assetReference.Guid);
                    var asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Object));
                    if (asset == null)
                    {
                        Debug.LogError($"Unreferenced {field.FieldType.Name}: {go.name}");
                        assetReference.Bundle = null;
                        assetReference.Name = null;
                        assetReference.Guid = null;
                        isValid = false;
                    }
                    else
                    {
                        assetReference.Name = asset.name;
                        assetReference.Bundle = AssetDatabase.GetImplicitAssetBundleName(assetPath);
                    }
                    
                    field.SetValue(mono, assetReference);
                }
            }
        }

        return isValid;
    }
}
