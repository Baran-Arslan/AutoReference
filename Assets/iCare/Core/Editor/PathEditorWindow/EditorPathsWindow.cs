using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace iCare.Editor.PathEditorWindow {
    public sealed class EditorPathsWindow : EditorWindow {
        [SerializeField] VisualTreeAsset visualTree;
        [SerializeField] StyleSheet styleSheet;

        const string WINDOW_TITLE = "EditorPaths";

        VisualElement _rootElement;
        ScrollView _prefabScrollView;
        ScrollView _scriptableObjectScrollView;

        [MenuItem("iCare/EditorPaths Window")]
        public static void ShowWindow() {
            var window = GetWindow<EditorPathsWindow>();
            window.titleContent = new GUIContent(WINDOW_TITLE);
        }


        void CreateGUI() {
            _rootElement = rootVisualElement;

            visualTree.CloneTree(_rootElement);

            _rootElement.styleSheets.Add(styleSheet);

            SetupUI();
            RegisterCallbacks();
        }

        void SetupUI() {
            _rootElement.Q<Label>("WindowTitle").text = "iCare Editor Paths";

            _prefabScrollView = _rootElement.Q<ScrollView>("PrefabScrollView");
            _scriptableObjectScrollView = _rootElement.Q<ScrollView>("ScriptableObjectScrollView");

            UpdatePathLists();
        }

        void RegisterCallbacks() {
            var prefabDropArea = _rootElement.Q<VisualElement>("PrefabDropArea");
            var scriptableObjectDropArea = _rootElement.Q<VisualElement>("ScriptableObjectDropArea");

            SetupDropArea(prefabDropArea, Paths.Prefab);
            SetupDropArea(scriptableObjectDropArea, Paths.ScriptableObject);
        }

        void SetupDropArea(VisualElement dropArea, Paths referenceType) {
            dropArea.RegisterCallback<DragEnterEvent>(_ => OnDragEnter(dropArea));
            dropArea.RegisterCallback<DragLeaveEvent>(_ => OnDragLeave(dropArea));
            dropArea.RegisterCallback<DragUpdatedEvent>(_ => OnDragUpdated());
            dropArea.RegisterCallback<DragPerformEvent>(_ => OnDragPerform(referenceType));
        }

        static void OnDragEnter(VisualElement dropArea) {
            dropArea.AddToClassList("drag-hover");
        }

        static void OnDragLeave(VisualElement dropArea) {
            dropArea.RemoveFromClassList("drag-hover");
        }

        static void OnDragUpdated() {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
        }

        void OnDragPerform(Paths referenceType) {
            DragAndDrop.AcceptDrag();

            foreach (var objectReference in DragAndDrop.objectReferences) {
                var path = AssetDatabase.GetAssetPath(objectReference);

                if (string.IsNullOrEmpty(path) || !AssetDatabase.IsValidFolder(path)) continue;

                var (success, message) = EditorPaths.AddPath(path, referenceType);
                if (!success) Debug.LogWarning(message);
            }

            UpdatePathLists();
        }

        void UpdatePathLists() {
            UpdatePathList(_prefabScrollView, Paths.Prefab.GetPaths(), Paths.Prefab);
            UpdatePathList(_scriptableObjectScrollView, Paths.ScriptableObject.GetPaths(),
                Paths.ScriptableObject);
        }

        void UpdatePathList(ScrollView scrollView, IEnumerable<string> paths, Paths referenceType) {
            scrollView.Clear();

            foreach (var path in paths) {
                var pathRow = new VisualElement();
                pathRow.AddToClassList("path-row");

                var pathLabel = new Label(path);
                pathLabel.AddToClassList("path-label");

                var removeButton = new Button(() => RemovePath(path, referenceType)) { text = "Remove" };
                removeButton.AddToClassList("remove-button");

                pathRow.Add(pathLabel);
                pathRow.Add(removeButton);

                scrollView.Add(pathRow);
            }
        }

        void RemovePath(string path, Paths referenceType) {
            if (EditorPaths.RemovePath(path, referenceType)) UpdatePathLists();
        }
    }
}