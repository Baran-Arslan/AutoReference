using System;
using System.Collections.Generic;
using System.Linq;
using iCare.Editor.PathEditorWindow;
using iCare.Utilities;
using UnityEditor;
using UnityEngine;

namespace iCare.Editor.EnumGeneration {
    public static class EnumTargetsGenerator {
        [MenuItem("iCare/Generate All Enums %#t")]
        public static void GenerateAll() {
            var scriptableObjects =
                AssetFinder.FindRegularAssets<ScriptableObject>(Paths.ScriptableObject);

            if (scriptableObjects.IsNullOrEmpty()) {
                Debug.LogWarning("No scriptable objects found to generate enum.");
                return;
            }

            var scriptableEnumMap = CreateScriptableEnumMap(scriptableObjects);
            Debug.Log($"Found {scriptableEnumMap.Count} enum types to generate.");

            GenerateEnums(scriptableEnumMap);
        }


        static Dictionary<Type, List<ScriptableObject>> CreateScriptableEnumMap(
            IReadOnlyCollection<ScriptableObject> scriptableObjects) {
            var scriptableEnumMap = new Dictionary<Type, List<ScriptableObject>>();

            foreach (var (scriptableType, enumType) in FindImplementingTypes()) {
                if (!scriptableEnumMap.ContainsKey(enumType))
                    scriptableEnumMap[enumType] = new List<ScriptableObject>();

                var matchingScriptableObjects = scriptableObjects.Where(so => so.GetType() == scriptableType);
                scriptableEnumMap[enumType].AddRange(matchingScriptableObjects);
            }

            return scriptableEnumMap;
        }

        static void GenerateEnums(Dictionary<Type, List<ScriptableObject>> scriptableEnumMap) {
            foreach (var kvp in scriptableEnumMap) {
                var enumType = kvp.Key;
                var scriptableObjects = kvp.Value;
                var enumValues = scriptableObjects.Select(so => so.name);
                EnumGenerator.GenerateByEnumType(enumType, enumValues);
            }
        }

        static IEnumerable<(Type scriptableType, Type enumType)> FindImplementingTypes() {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(ScriptableObject).IsAssignableFrom(type))
                .Select(type => (scriptableType: type, enumType: GetScriptableEnumGenericType(type)))
                .Where(tuple => tuple.enumType != null);
        }

        static Type GetScriptableEnumGenericType(Type type) {
            return type.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumTarget<>))
                .Select(i => i.GetGenericArguments()[0])
                .FirstOrDefault();
        }
    }
}