using System;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif


namespace iCare.AutoReference {
    [AttributeUsage(AttributeTargets.Field)]
    public abstract class ReferenceAttribute : PropertyAttribute {
        protected ReferenceAttribute(string id = null) {
            ID = id;
        }

        public string ID { get; }
    }

#if ODIN_INSPECTOR
    [IncludeMyAttributes] [ReadOnly] [FoldoutGroup("Dependencies")]
#endif
    public sealed class AutoAttribute : ReferenceAttribute {
        public AutoAttribute(string id = null) : base(id) { }
    }

#if ODIN_INSPECTOR
    [IncludeMyAttributes] [ReadOnly] [FoldoutGroup("Dependencies")]
#endif
    public sealed class AutoListAttribute : ReferenceAttribute {
        public AutoListAttribute(string id = null) : base(id) { }
    }
}