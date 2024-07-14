using System;
using System.Diagnostics;
using Sirenix.OdinInspector;

namespace iCare {
    [IncludeMyAttributes] [ChildGameObjectsOnly] [Required] [Conditional("UNITY_EDITOR")]
    public sealed class InChildrensAttribute : Attribute { }
}