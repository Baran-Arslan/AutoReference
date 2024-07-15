using UnityEngine;
using UnityEngine.UIElements;

namespace iCare.AutoReference.Extra.PrefabService.Editor {
    [UnityEditor.CustomEditor(typeof(AutoReference.PrefabService))]
    internal sealed class PrefabServiceEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI() {
            var root = new VisualElement();
            
            //Write Prefab Service with big letters and bold and blue background as header
            var header = new Label("Prefab Service") {
                style = {
                    fontSize = 20,
                    unityFontStyleAndWeight = FontStyle.Bold,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    backgroundColor = new Color(0.16f, 0.3f, 1f)
                }
            };
            
            
            //Add space
            
            //Add description text under it
            var description = new Label("All components in this game object will be registered as service.")
            {
                style = {
                    unityTextAlign = TextAnchor.LowerLeft,
                    unityFontStyleAndWeight = FontStyle.Bold,
                    fontSize = 14,
                    backgroundColor = new Color(0.16f, 0.3f, 1f)
                }
            };
            
            
            
            
            
            root.Add(header);
            root.Add(description);
            
            
            return root;
        }
    }
}
