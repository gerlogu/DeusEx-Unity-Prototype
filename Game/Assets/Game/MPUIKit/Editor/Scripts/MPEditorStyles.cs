using System.IO;
using UnityEditor;
using UnityEngine;

namespace MPUIKIT.Editor {
    [InitializeOnLoad]
    internal static class MPEditorStyles {
        private static string _mpuiKitIconsDirectory = string.Empty;

        private static GUIContent _flipHorizontalNormal, _flipHorizontalActive;
        private static GUIContent _flipVerticalNormal, _flipVerticalActive;

        private static GUIContent _rotateLeftNormal, _rotateLeftActive;
        private static GUIContent _rotateRightNormal, _rotateRightActive;

        public static GUIContent FlipHorizontalNormal {
            get {
                if (_flipHorizontalNormal != null) return _flipHorizontalNormal;
                _flipHorizontalNormal = new GUIContent(LoadImage("flip_h", false));
                return _flipHorizontalNormal;
            }
        }

        public static GUIContent FlipHorizontalActive {
            get {
                if (_flipHorizontalActive != null) return _flipHorizontalActive;
                _flipHorizontalActive = new GUIContent(LoadImage("flip_h", true));
                return _flipHorizontalActive;
            }
        }

        public static GUIContent FlipVerticalNormal {
            get {
                if (_flipVerticalNormal != null) return _flipVerticalNormal;
                _flipVerticalNormal = new GUIContent(LoadImage("flip_v", false));
                return _flipVerticalNormal;
            }
        }

        public static GUIContent FlipVerticalActive {
            get {
                if (_flipVerticalActive != null) return _flipVerticalActive;
                _flipVerticalActive = new GUIContent(LoadImage("flip_v", true));
                return _flipVerticalActive;
            }
        }

        public static GUIContent RotateLeftNormal {
            get {
                if (_rotateLeftNormal != null) return _rotateLeftNormal;
                _rotateLeftNormal = new GUIContent(LoadImage("rotate_left", false));
                return _rotateLeftNormal;
            }
        }

        public static GUIContent RotateLeftActive {
            get {
                if (_rotateLeftActive != null) return _rotateLeftActive;
                _rotateLeftActive = new GUIContent(LoadImage("rotate_left", true));
                return _rotateLeftActive;
            }
        }

        public static GUIContent RotateRightNormal {
            get {
                if (_rotateRightNormal != null) return _rotateRightNormal;
                _rotateRightNormal = new GUIContent(LoadImage("rotate_right", false));
                return _rotateRightNormal;
            }
        }

        public static GUIContent RotateRightActive {
            get {
                if (_rotateRightActive != null) return _rotateRightActive;
                _rotateRightActive = new GUIContent(LoadImage("rotate_right", true));
                return _rotateRightActive;
            }
        }

        static MPEditorStyles() {
            FindMpuiKitIconsDirectory();
        }

        private static void FindMpuiKitIconsDirectory() {
            string[] ids = AssetDatabase.FindAssets("MPImageEditor t:Script");
            string mpImageEditorScriptPath = string.Empty;
            foreach (string id in ids) {
                string assetPath = AssetDatabase.GUIDToAssetPath(id);
                if (!assetPath.Contains($"MPUIKit/Editor/Scripts/MPImageEditor.cs") &&
                    !assetPath.Contains($"MPUIKit\\Editor\\Scripts\\MPImageEditor.cs")) continue;
                mpImageEditorScriptPath = assetPath;
            }

            if (string.IsNullOrEmpty(mpImageEditorScriptPath)) {
                return;
            }

            DirectoryInfo iconDirectory = new DirectoryInfo(mpImageEditorScriptPath);
            iconDirectory = iconDirectory.Parent.Parent;
            DirectoryInfo unityRoot = new DirectoryInfo(Application.dataPath).Parent;

            _mpuiKitIconsDirectory = Path.Combine(iconDirectory.ToString(), $"Icons{Path.DirectorySeparatorChar}")
                .Replace(unityRoot.ToString() + Path.DirectorySeparatorChar, string.Empty);
        }

        private static Texture2D LoadImage(string name, bool activeState) {
            int colorLevel = 0;
#if UNITY_2019_3_OR_NEWER
            if (activeState) colorLevel = 3;
            else colorLevel = EditorGUIUtility.isProSkin ? 2 : 1;
#else
            //TODO: Fix the color levels for older unity editors
            if(activeState) colorLevel = 3;
            else colorLevel = EditorGUIUtility.isProSkin ? 2 : 1;
#endif
            if (_mpuiKitIconsDirectory == string.Empty) {
                FindMpuiKitIconsDirectory();
            }

            return AssetDatabase.LoadAssetAtPath($"{_mpuiKitIconsDirectory}{name}_{colorLevel}.png", typeof(Texture2D))
                as Texture2D;
        }
    }
}