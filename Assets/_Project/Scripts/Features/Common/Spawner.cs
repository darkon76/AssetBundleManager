using Scripts.Core.AssetBundleManager;
using UnityEngine;

namespace Scripts.Features.Common
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private AssetReference _assetReference;

        private void Start()
        {
            var go = AssetLoader.Instance.Instantiate<GameObject>(_assetReference, transform.parent);
            if (go != null)
            {
                go.transform.SetPositionAndRotation(transform.position, transform.rotation);
            }
            //Destroy(gameObject);
        }
    }
}