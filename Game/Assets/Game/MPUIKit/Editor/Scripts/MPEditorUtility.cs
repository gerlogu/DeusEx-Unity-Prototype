using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace MPUIKIT.Editor {
    public static class MPEditorUtility {
        public static void CornerRadiusModeGUI(Rect rect, ref SerializedProperty property, string[] toolBarHeading,
            string label = "Corner Radius") {
            bool boolVal = property.boolValue;
            Rect labelRect = new Rect(rect.x, rect.y, EditorGUIUtility.labelWidth, rect.height);
            Rect toolBarRect = new Rect(rect.x + EditorGUIUtility.labelWidth, rect.y,
                rect.width - EditorGUIUtility.labelWidth, rect.height);

            EditorGUI.BeginChangeCheck();
            {
                EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
                EditorGUI.LabelField(labelRect, label);

                boolVal = GUI.Toolbar(toolBarRect, boolVal ? 1 : 0, toolBarHeading) == 1;
                EditorGUI.showMixedValue = false;
            }
            if (EditorGUI.EndChangeCheck()) {
                property.boolValue = boolVal;
            }
        }

        [MenuItem("GameObject/UI/MPImage")]
        public static void AddMPImageObject() {
            GameObject g = new GameObject {name = "MPImage"};
            Transform parent;
            if (Selection.activeGameObject != null &&
                Selection.activeGameObject.GetComponentInParent<Canvas>() != null) {
                parent = Selection.activeGameObject.transform;
            }
            else {
                if (!Object.FindObjectOfType<Canvas>()) {
                    EditorApplication.ExecuteMenuItem("GameObject/UI/Canvas");
                }

                Canvas c = Object.FindObjectOfType<Canvas>();
                AddAdditionalShaderChannelsToCanvas(c);
                parent = c.transform;
            }

            g.transform.SetParent(parent, false);
            g.AddComponent<MPImage>();
            Selection.activeGameObject = g;
        }

        [MenuItem("CONTEXT/Image/Replace with MPImage")]
        public static void ReplaceWithMPImage(MenuCommand command) {
            Image img = (Image) command.context;
            GameObject obj = img.gameObject;
            Object.DestroyImmediate(img);
            obj.AddComponent<MPImage>();
        }

        internal static void AddAdditionalShaderChannelsToCanvas(Canvas c) {
            AdditionalCanvasShaderChannels additionalShaderChannels = c.additionalShaderChannels;
            additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord1;
            additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord2;
            c.additionalShaderChannels = additionalShaderChannels;
        }

        internal static bool HasAdditionalShaderChannels(Canvas c) {
            AdditionalCanvasShaderChannels asc = c.additionalShaderChannels;
            return (asc & AdditionalCanvasShaderChannels.TexCoord1) != 0 &&
                   (asc & AdditionalCanvasShaderChannels.TexCoord2) != 0;
        }
        
    }
}