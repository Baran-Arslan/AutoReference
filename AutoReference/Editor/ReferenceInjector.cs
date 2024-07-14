using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using iCare.Editor;
using iCare.Editor.PathEditorWindow;
using iCare.Utilities;
using UnityEditor;
using UnityEngine;


namespace iCare.AutoReference.Editor {
    internal static class ReferenceInjector {
        [MenuItem("iCare/Inject Everything #^e")]
        public static void InjectEverything() {
            var allObjects = GetAllObjects();

            var allServices = allObjects.OfType<IProvideReference>();
            var serviceResolver = new DependencyResolver(allServices);

            if (allObjects.IsNullOrEmpty())
                throw new System.Exception("No injectable objects found.");

            Debug.Log($"Found total of {allObjects.Count} injectable objects.");

            foreach (var target in allObjects) {
                if (target != null) 
                    Inject(target, serviceResolver);
            }
        }


        static IReadOnlyList<Object> GetAllObjects() {
            var allObjects = new List<Object>();

            var allSo = AssetFinder.FindRegularAssets<ScriptableObject>(Paths.ScriptableObject);
            var allPrefabScripts = AssetFinder.FindComponentsInPrefabs<MonoBehaviour>(Paths.Prefab);
            var allSceneScripts = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

            allObjects.AddRange(allSo);
            allObjects.AddRange(allPrefabScripts);
            allObjects.AddRange(allSceneScripts);
            return allObjects;
        }

        static void Inject(Object target, DependencyResolver serviceResolver) {
            var fields = GetFieldsWithReferenceAttribute(target);
            if (fields.IsNullOrEmpty())
                return;

            Debug.Log($"Injecting into {target.name.Highlight()} with {fields.Count.ToString().Highlight()} fields.",target);

            foreach (var field in fields) {
                var service = serviceResolver.Resolve(field);
                field.Inject(target, service);
            }

            EditorUtility.SetDirty(target);
        }

        static IReadOnlyCollection<FieldInfo> GetFieldsWithReferenceAttribute(this Object target) {
            var fields = new List<FieldInfo>();
            var type = target.GetType();

            while (type != null )
            {
                fields.AddRange(type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                    .Where(field => field.GetCustomAttribute<ReferenceAttribute>() != null));
                if (type.BaseType == typeof(MonoBehaviour))
                    break;
                if (type.BaseType == typeof(ScriptableObject))
                    break;
                
                type = type.BaseType;
            }

            return fields;
        }  

        #region Test Wrappers

        internal static void InjectWrapper(Object target, DependencyResolver serviceResolver) {
            Inject(target, serviceResolver);
        }

        #endregion
    }
}