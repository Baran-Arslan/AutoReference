using System.Diagnostics;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace iCare {
    [IncludeMyAttributes] [CanBeNull] [GUIColor(0.75f, 1f, 0.73f)] [Tooltip("This field can stay null!")]
    [Conditional("UNITY_EDITOR")]
    public sealed class CanStayNullAttribute : OptionalAttribute { }
}