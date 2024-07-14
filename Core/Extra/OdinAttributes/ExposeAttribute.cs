using System;
using System.Diagnostics;
using Sirenix.OdinInspector;

namespace iCare {
    [ShowInInspector] [ReadOnly] [Conditional("UNITY_EDITOR")]
    public sealed class ExposeAttribute : Attribute { }
}