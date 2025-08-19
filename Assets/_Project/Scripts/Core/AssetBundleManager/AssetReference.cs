using System;

namespace Scripts.Core.AssetBundleManager
{
    /// <summary>
    /// The class uses a property drawer to select an object to populate the name and bundle.
    /// The current biggest downside is if the asset name or bundle changes the values from this class aren't automatically validated.
    /// It will require a postprocess script or asset validation before building, both outside of the scope of the test. 
    /// </summary>
    [Serializable]
    public class AssetReference
    {
        public string Name;
        public string Bundle;
        //Because the asset can be moved around and change name we use the unity GUID as our source of truth.   
#if UNITY_EDITOR
        public string Guid;
#endif
    }
}