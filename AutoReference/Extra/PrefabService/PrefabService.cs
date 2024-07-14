#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace iCare.AutoReference {
#if ODIN_INSPECTOR
    [HideMonoScript]
#endif
    public sealed class PrefabService : MonoBehaviour, IProvideReference, IEnumTarget<Prefabs> {
        public object RefService => gameObject;
        [Expose] public string RefID => gameObject.name;

        void Awake() {
            Destroy(this);
        }


        public IEnumerable<Type> RefTypes {
            get {
                List<Type> typeList = new() { typeof(GameObject) };
                var compsInSelf = GetComponents<Component>();
                foreach (var comp in compsInSelf) typeList.AddRange(GetTypesInComponent(comp));

                return typeList;
            }
        }

        static IEnumerable<Type> GetTypesInComponent(Component comp) {
            yield return comp.GetType();
            if (comp is not MonoBehaviour) yield break;
            foreach (var @interface in comp.GetType().GetInterfaces()) yield return @interface;
        }
    }
}


#endif