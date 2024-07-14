using System;
using System.Diagnostics;
using Sirenix.OdinInspector;

namespace iCare {
    [IncludeMyAttributes] [AssetsOnly] [Required] [Conditional("UNITY_EDITOR")]
    public sealed class InAssetsAttribute : Attribute { }
}