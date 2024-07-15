using System;
using System.Collections.Generic;
using System.Linq;
using iCare.Editor.PathEditorWindow;
using iCare.Utilities;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object; 
    
namespace iCare.Editor.EnumGeneration {
    public static class EnumTargetsGenerator {
        [MenuItem("iCare/Generate All Enums %#t")]
        public static void GenerateAll() {
            var allObjects = new List<Object>();
            allObjects.AddRange(AssetFinder.FindRegularAssets<ScriptableObject>(Paths.ScriptableObject));
            allObjects.AddRange(AssetFinder.FindComponentsInPrefabs<MonoBehaviour>(Paths.Prefab));

            if (allObjects.IsNullOrEmpty()) {
                Debug.LogWarning("No scriptable objects found to generate enum.");
                return;
            }

            var scriptableEnumMap = CreateObjectEnumMap(allObjects);

            foreach (var kvp in scriptableEnumMap) {
                Debug.Log("Generating enum for: " + kvp.Key.Name);
            }
            
            GenerateEnums(scriptableEnumMap);
        }

        static Dictionary<Type, List<Object>> CreateObjectEnumMap(IReadOnlyCollection<Object> objects) {
            var objectEnumMap = new Dictionary<Type, List<Object>>();

            foreach (var (objectType, enumType) in FindImplementingTypes()) {
                if (!objectEnumMap.ContainsKey(enumType))
                    objectEnumMap[enumType] = new List<Object>();

                var matchingObjects = objects.Where(obj => obj.GetType() == objectType);
                objectEnumMap[enumType].AddRange(matchingObjects);
            }

            return objectEnumMap;
        }

        static void GenerateEnums(Dictionary<Type, List<Object>> objectEnumMap) {
            foreach (var kvp in objectEnumMap) {
                var enumType = kvp.Key;
                var objects = kvp.Value;
                var enumValues = objects.Select(obj => obj.name);
                EnumGenerator.GenerateByEnumType(enumType, enumValues);
            }
        }

        static IEnumerable<(Type objectType, Type enumType)> FindImplementingTypes() {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Select(type => (objectType: type, enumType: GetObjectEnumGenericType(type)))
                .Where(tuple => tuple.enumType != null);
        }

        static Type GetObjectEnumGenericType(Type type) {
            return type.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumTarget<>))
                .Select(i => i.GetGenericArguments()[0])
                .FirstOrDefault();
        }
    }
}
