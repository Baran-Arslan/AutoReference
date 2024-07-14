using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace iCare.Utilities {
    public static class NullExtensions {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list) {
            return list == null || !list.Any();
        }

        public static bool IsUnityNull(this object obj) {
            return obj == null || (obj is Object unityObj && unityObj == null);
        }

        public static bool IsNotNullAndActive(this Component comp) {
            return comp != null && comp.gameObject.activeSelf;
        }
    }
}