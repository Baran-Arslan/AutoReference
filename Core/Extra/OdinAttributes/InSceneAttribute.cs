using System;
using System.Diagnostics;
using Sirenix.OdinInspector;

namespace iCare {
    [IncludeMyAttributes] [SceneObjectsOnly] [Required] [Conditional("UNITY_EDITOR")]
    public sealed class InSceneAttribute : Attribute { }
}