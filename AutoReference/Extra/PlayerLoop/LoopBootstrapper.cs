using iCare.Utilities;
using UnityEngine;

namespace iCare.AutoReference {
    internal static class LoopBootstrapper {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        internal static void Bootstrap() {
            var loopManagerPrefab = Resources.Load<GameObject>("iCare");
            if (loopManagerPrefab == null)
                throw new System.Exception("iCare.prefab not found in Resources folder".Highlight());
            var loopManager = Object.Instantiate(loopManagerPrefab);
            Object.DontDestroyOnLoad(loopManager);
        }
    }
}