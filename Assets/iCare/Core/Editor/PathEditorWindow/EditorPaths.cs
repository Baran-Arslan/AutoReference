using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace iCare.Editor.PathEditorWindow {
    internal static class EditorPaths {
        const string PREFERENCE_KEY = "EditorPaths";
        const string PREFAB_PREFIX = "Prefab:";
        const string SCRIPTABLE_OBJECT_PREFIX = "ScriptableObject:";

        static readonly List<string> _prefabPaths = new List<string>();
        static readonly List<string> _scriptableObjectPaths = new List<string>();

        static void LoadPaths() {
            _prefabPaths.Clear();
            _scriptableObjectPaths.Clear();

            var pathsString = EditorPrefs.GetString(PREFERENCE_KEY);
            if (string.IsNullOrEmpty(pathsString)) return;

            foreach (var path in pathsString.Split(';'))
                if (path.StartsWith(PREFAB_PREFIX))
                    _prefabPaths.Add(path.Substring(PREFAB_PREFIX.Length));
                else if (path.StartsWith(SCRIPTABLE_OBJECT_PREFIX))
                    _scriptableObjectPaths.Add(path.Substring(SCRIPTABLE_OBJECT_PREFIX.Length));
        }

        static void SavePaths() {
            var allPaths = new List<string>();
            allPaths.AddRange(_prefabPaths.Select(path => $"{PREFAB_PREFIX}{path}"));
            allPaths.AddRange(_scriptableObjectPaths.Select(path => $"{SCRIPTABLE_OBJECT_PREFIX}{path}"));
            EditorPrefs.SetString(PREFERENCE_KEY, string.Join(";", allPaths));
        }

        static bool IsValidPath(string path) {
            return !string.IsNullOrEmpty(path) && path != "Assets";
        }

        static bool IsPathDuplicateOrNested(string newPath, Paths type) {
            return GetPathList(type).Any(existingPath => IsPathNestedOrEqual(newPath, existingPath));
        }

        static bool IsPathNestedOrEqual(string path1, string path2) {
            return path1 == path2 || path1.StartsWith(path2 + "/") || path2.StartsWith(path1 + "/");
        }

        static List<string> GetPathList(Paths type) {
            LoadPaths();
            return type == Paths.Prefab ? _prefabPaths : _scriptableObjectPaths;
        }

        internal static string[] GetPaths(this Paths type) {
            return GetPathList(type).ToArray();
        }

        internal static (bool success, string message) AddPath(string path, Paths type) {
            if (!IsValidPath(path)) return (false, "Invalid path. Cannot add root folder or empty path.");

            if (IsPathDuplicateOrNested(path, type)) return (false, "Path is duplicate or nested.");

            GetPathList(type).Add(path);
            SavePaths();
            return (true, "Path added successfully.");
        }

        internal static bool RemovePath(string path, Paths type) {
            if (!GetPathList(type).Remove(path)) return false;
            SavePaths();
            return true;
        }
    }
}