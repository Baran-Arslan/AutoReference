using System;
using System.Collections.Generic;
using System.Linq;
using iCare.Attributes;
using UnityEditor;
using UnityEngine.UIElements;

namespace iCare.Editor.PropertyDrawers {
    [CustomPropertyDrawer(typeof(HideEnumAttribute), true)]
    public sealed class HideEnumPropertyDrawer : PropertyDrawer {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();
    
            if (fieldInfo == null)
            {
                container.Add(new Label("Error: fieldInfo is null"));
                return container;
            }

            var enumType = fieldInfo.FieldType;
            var hideEnumAttribute = attribute as HideEnumAttribute;
    
            if (hideEnumAttribute == null)
            {
                container.Add(new Label("Error: HideEnumAttribute is null"));
                return container;
            }

            var hiddenValues = hideEnumAttribute.HiddenValues?.Cast<Enum>() ?? Enumerable.Empty<Enum>();

            var displayedOptions = GetDisplayOptions(enumType, hiddenValues);

            if (displayedOptions.Count == 0)
            {
                container.Add(new Label("No options available"));
                return container;
            }

            var currentEnumValue = Enum.ToObject(enumType, property.enumValueIndex) as Enum;
            var currentIndex = currentEnumValue != null ? displayedOptions.IndexOf(currentEnumValue) : 0;
            currentIndex = Math.Max(0, currentIndex); // Ensure index is not -1

            var popupField = new PopupField<Enum>(property.displayName, displayedOptions, currentIndex)
            {
                formatListItemCallback = item => item?.ToString() ?? "Null",
                formatSelectedValueCallback = item => item?.ToString() ?? "Null"
            };

            popupField.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue != null)
                {
                    property.enumValueIndex = Array.IndexOf(Enum.GetValues(enumType), evt.newValue);
                    property.serializedObject.ApplyModifiedProperties();
                }
            });

            container.Add(popupField);

            return container;
        }

        static List<Enum> GetDisplayOptions(Type enumType, IEnumerable<Enum> hiddenValues) {
            return Enum.GetValues(enumType)
                .Cast<Enum>()
                .Where(e => !hiddenValues.Contains(e))
                .ToList();
        }
    }
}