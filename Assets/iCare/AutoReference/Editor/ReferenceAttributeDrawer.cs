using UnityEditor;
using UnityEngine;

namespace iCare.AutoReference.Editor {
    [CustomPropertyDrawer(typeof(ReferenceAttribute), true)]
    internal sealed class ReferenceAttributeDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var originalColor = GUI.backgroundColor;

            GUI.backgroundColor = property.objectReferenceValue == null ? Color.red : Color.green;

            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label);
            GUI.backgroundColor = originalColor;
            GUI.enabled = true;
        }
    }
}