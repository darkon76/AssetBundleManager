using System;

[Serializable]
public class AssetReference
{
    
    public string Name;
    public string Bundle;
    //Because the asset can be moved around and change name we use the unity GUID as our source of truth.   
#if UNITY_EDITOR
    public string guid;
#endif

}
