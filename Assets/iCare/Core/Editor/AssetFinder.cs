using System.Collections.Generic;
using System.Linq;
using iCare.Editor.PathEditorWindow;
using UnityEditor;
using UnityEngine;

namespace iCare.Editor {
    public static class AssetFinder {
        public static IReadOnlyCollection<T> FindRegularAssets<T>(params string[] searchFolders) where T : Object {
            return AssetDatabase.FindAssets($"t:{typeof(T).Name}", searchFolders)
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<T>)
                .Where(asset => asset != null)
                .ToArray();
        }

        public static IReadOnlyCollection<T> FindComponentsInPrefabs<T>(params string[] searchFolders)
            where T : Component {
            return AssetDatabase.FindAssets("t:Prefab", searchFolders)
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<GameObject>)
                .Where(prefab => prefab != null)
                .SelectMany(prefab => prefab.GetComponentsInChildren<T>(true))
                .ToArray();
        }

        public static IReadOnlyCollection<T> FindRegularAssets<T>(Paths pathType) where T : Object {
            return FindRegularAssets<T>(pathType.GetPaths());
        }

        public static IReadOnlyCollection<T> FindComponentsInPrefabs<T>(Paths pathType) where T : Component {
            return FindComponentsInPrefabs<T>(pathType.GetPaths());
        }
    }
}