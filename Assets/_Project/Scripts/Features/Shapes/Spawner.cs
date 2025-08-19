using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] 
    private AssetReference _assetReference;

    private void Start()
    {
        var go = AssetBundleCoordinator.Instance.Instatiate<GameObject>(_assetReference, transform);
        if (go != null)
        {
            go.transform.SetPositionAndRotation(transform.position, transform.rotation);
        }
    }
}
