using UnityEngine;

namespace iCare.AutoReference.Demo.PrefabExample {
    public sealed class PrefabRequirer : MonoBehaviour
    {
        [Auto(nameof(Prefabs.PrefabExample_Projectile))]
        public GameObject prefabService;
        
        [Auto(nameof(Prefabs.PrefabExample_Projectile))]
        public Rigidbody rigidbodyService;
        
        [Auto(nameof(Prefabs.PrefabExample_Projectile))]
        public Collider colliderService;
        
        [Auto(nameof(Prefabs.PrefabExample_Projectile))]
        public Transform transformService;
        
        [Auto(nameof(Prefabs.PrefabExample_Projectile))]
        public PrefabService prefabServiceService;
    }
}
