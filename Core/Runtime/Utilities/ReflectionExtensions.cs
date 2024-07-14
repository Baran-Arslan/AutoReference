using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace iCare.Utilities {
    public static class ReflectionExtensions {
        public static bool IsList(this Type type) {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
        }

        public static bool IsHashSet(this Type type) {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(HashSet<>);
        }

        public static bool IsIEnumerable(this Type type) {
            return type.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }

        public static Type GetElementType(this FieldInfo field) {
            var fieldType = field.FieldType;
            if (fieldType.IsList() || fieldType.IsHashSet()) return fieldType.GetGenericArguments()[0];

            if (fieldType.IsArray)
                return fieldType.GetElementType();

            if (fieldType.IsIEnumerable() && fieldType != typeof(string)) {
                Debug.LogWarning("Unsupported IEnumerable type. Using first generic argument.".Highlight());
                return fieldType.GetInterfaces()
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    .GetGenericArguments()[0];
            }

            return fieldType;
        }
    }
}