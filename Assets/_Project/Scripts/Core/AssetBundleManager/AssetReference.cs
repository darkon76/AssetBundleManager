using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class AssetReference
{
    
    public string name;
    public string bundle;
    
#if UNITY_EDITOR
    public string guid;
#endif
}
