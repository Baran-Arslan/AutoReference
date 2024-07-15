using System;
using UnityEngine;

namespace iCare.AutoReference {
    [AttributeUsage(AttributeTargets.Field)]
    public abstract class ReferenceAttribute : PropertyAttribute {
        protected ReferenceAttribute(string id = null) {
            ID = id;
        }

        public string ID { get; }
    }

    public sealed class AutoAttribute : ReferenceAttribute {
        public AutoAttribute(string id = null) : base(id) { }
    }

    public sealed class AutoListAttribute : ReferenceAttribute {
        public AutoListAttribute(string id = null) : base(id) { }
    }
}