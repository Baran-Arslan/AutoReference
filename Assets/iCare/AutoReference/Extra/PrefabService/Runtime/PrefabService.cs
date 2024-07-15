
using System;
using System.Collections.Generic;
using UnityEngine;

namespace iCare.AutoReference {
    public sealed class PrefabService : MonoBehaviour, IProvideReference, IEnumTarget<Prefabs> {
        public object RefService => gameObject;
        public string RefID => gameObject.name;

        void Awake() {
            Destroy(this);
        }


        public IEnumerable<Type> RefTypes {
            get {
                List<Type> typeList = new List<Type> { typeof(GameObject) };
                var compsInSelf = GetComponents<Component>();
                foreach (var comp in compsInSelf) typeList.AddRange(GetTypesInComponent(comp));

                return typeList;
            }
        }

        static IEnumerable<Type> GetTypesInComponent(Component comp) {
            yield return comp.GetType();
            if (!(comp is MonoBehaviour)) yield break;
            foreach (var @interface in comp.GetType().GetInterfaces()) yield return @interface;
        }
    }
}
