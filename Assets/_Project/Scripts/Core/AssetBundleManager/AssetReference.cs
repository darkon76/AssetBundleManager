using System;

namespace Scripts.Core.AssetBundleManager
{
    /// <summary>
    /// The class uses a property drawer to select an object to populate the name and bundle of the object that is referenced.
    /// The current biggest downside of this implementation is if the asset name or bundle changes,
    /// the user needs to manually select the objects that have a references to the edited object.
    /// To automate the process it will require a postprocess script or doing an asset validation before building.
    /// </summary>
    [Serializable]
    public class AssetReference
    {
        //Because the asset can be moved around and change name we use the unity GUID as our source of truth.   
#if UNITY_EDITOR
        public string Guid;
#endif
        //The name and bundle are currently populated by the property drawer. 
        public string Name;
        public string Bundle;
        
    }
}