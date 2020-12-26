using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace MPUIKIT.Editor {
    [InitializeOnLoad]
    public class MPUIKitSetupWindow : EditorWindow {

        private static SerializedObject _graphicsSettingsObj;
        private static bool _setup;
        private static bool _alreadyShownOnStartup;
        private bool _initialized;
        private string _version = "Version: 0.92 (Beta)";

        private const string AlreadyShownOnStartupKey = "MPUtilityAlreadyShown";
        private static SerializedObject GraphicsSettingsObj {
            get {
                if (_graphicsSettingsObj != null) return _graphicsSettingsObj;
                _graphicsSettingsObj = new SerializedObject(
                    AssetDatabase.LoadAssetAtPath<GraphicsSettings>("ProjectSettings/GraphicsSettings.asset"));
                return _graphicsSettingsObj;
            }
        }

        static MPUIKitSetupWindow() {
            EditorApplication.update += HandleOnStartup;
        }

        private static void CheckSetup() {
            if (!_setup) {
                EditorApplication.ExecuteMenuItem("Window/MPUIKit/Utility Panel");
            }
        }

        private static void HandleOnStartup() {
            EditorApplication.update -= HandleOnStartup;
            EditorApplication.wantsToQuit += HandleOnWantsToQuit;
            _setup = IsShaderAdded(MPImage.MpShaderName);
            _alreadyShownOnStartup = EditorPrefs.GetBool(AlreadyShownOnStartupKey);

            if (!_setup) {
                Debug.LogWarning("MPUIKit is not setup in the project. Go to 'Window/MPUIKit/Utility Panel' to set it up.");
            }
            if (_alreadyShownOnStartup) return;
            CheckSetup();
        }

        private static bool HandleOnWantsToQuit() {
            EditorApplication.wantsToQuit -= HandleOnWantsToQuit;
            EditorPrefs.DeleteKey(AlreadyShownOnStartupKey);
            return true;
        }

        private static bool IsShaderAdded(string shaderName) {
            Shader shader = Shader.Find(shaderName);
            if (shader == null)
                return false;

            
            SerializedProperty arrayProp = GraphicsSettingsObj.FindProperty("m_AlwaysIncludedShaders");
            
            bool hasShader = false;
            for (int i = 0; i < arrayProp.arraySize; ++i)
            {
                SerializedProperty arrayElem = arrayProp.GetArrayElementAtIndex(i);
                if (shader != arrayElem.objectReferenceValue) continue;
                hasShader = true;
                break;
            }

            return hasShader;
        }

        private static void AddShader(string shaderName) {
            Shader shader = Shader.Find(shaderName);
            if (shader == null)
                return;
            
            SerializedProperty prop = GraphicsSettingsObj.FindProperty("m_AlwaysIncludedShaders");
            
            int arrayIndex = prop.arraySize;
            prop.InsertArrayElementAtIndex(arrayIndex);
            SerializedProperty arrayElem = prop.GetArrayElementAtIndex(arrayIndex);
            arrayElem.objectReferenceValue = shader;
 
            GraphicsSettingsObj.ApplyModifiedProperties();
 
            AssetDatabase.SaveAssets();
        }


        [MenuItem("Window/MPUIKit/Utility Panel")]
        public static void ShowWindow() {
            EditorWindow window = GetWindow<MPUIKitSetupWindow>(true, "MPUIKit Utility Panel", true);
            window.maxSize = new Vector2(350, 300); // (350, 350);
            window.minSize = window.maxSize;
            EditorPrefs.SetBool(AlreadyShownOnStartupKey, true);
        }
        

        private void Initialize() {
            if (_initialized) return;
            _initialized = true;
            _setup = IsShaderAdded(MPImage.MpShaderName);
        }

        private void OnGUI() {
            Initialize();
            GUILayout.Space(30);
            Rect buttonRect = EditorGUILayout.GetControlRect(false, 40);
            float buttonWidth = ((buttonRect.xMax - buttonRect.xMin) - 6) / 2;
            Color currentColor = GUI.color;
            if (_setup) {
                GUI.color = Color.green;
            }
            else {
                GUI.color = Color.yellow;
            }
            
            if (GUI.Button(new Rect(buttonRect.width / 2 - 100, buttonRect.y + buttonRect.height / 2 - 20, 200, 45), "Setup MPUIKit")) {
                if (!IsShaderAdded(MPImage.MpShaderName)) {
                    AddShader(MPImage.MpShaderName);
                    _setup = true;
                }
            }

            if (_setup) {
                GUILayout.Space(8);
                EditorGUILayout.LabelField("MPUIKit is set up successfully.", EditorStyles.centeredGreyMiniLabel);
            }
            else {
                GUILayout.Space(8);
                EditorGUILayout.LabelField("MPUIKit is not set up in the project.", EditorStyles.centeredGreyMiniLabel);
            }
            

            // GUI.color = Color.cyan;
            // GUILayout.Space(10);
            // buttonRect = EditorGUILayout.GetControlRect(GUILayout.Height(30), GUILayout.ExpandWidth(true));
            //
            // if (GUI.Button(new Rect(buttonRect.width / 2 - 100, buttonRect.y + buttonRect.height / 2 - 15, 200, 30), "Create Assembly Definitions")) {
            //     AddAssemblyDefinitions();
            // }

            GUI.color = currentColor;
            GUILayout.Space(12);

            buttonRect = EditorGUILayout.GetControlRect(false, 40*3+4);
            buttonRect.width = (buttonRect.width / 2) - 1;
            buttonRect.height = 40;

            if (GUI.Button(buttonRect, "Documentation")) {
                Application.OpenURL("https://scrollbie.com/mpuikit/documentation.html");
            }

            buttonRect.x += buttonRect.width + 2;
            if (GUI.Button(buttonRect, "Website")) {
                Application.OpenURL("https://scrollbie.com/mpuikit/index.html");
            }

            buttonRect.y += 42;
            buttonRect.x -= buttonRect.width + 2;
            if (GUI.Button(buttonRect, "Email")) {
                Application.OpenURL("mailto:support@scrollbie.com");
            }

            buttonRect.x += buttonRect.width + 2;
            if (GUI.Button(buttonRect, "Forum")) {
                Application.OpenURL("https://forum.unity.com/threads/an-advanced-procedural-ui-generation-tool-create-modify-animate-spriteless-ui-even-at-runtime.846772");
            }
            
            buttonRect.y += 42;
            buttonRect.x -= buttonRect.width + 2;
            if (GUI.Button(buttonRect, "Changelog")) {
                Application.OpenURL("https://scrollbie.com/mpuikit/changelog.html");
            }

            buttonRect.x += buttonRect.width + 2;
            if (GUI.Button(buttonRect, "Other Assets")) {
                Application.OpenURL("https://assetstore.unity.com/publishers/29536");
            }
            
            if (GUILayout.Button("Rate MPUIKit", GUILayout.ExpandWidth(true), GUILayout.Height(40))) {
                Application.OpenURL("https://assetstore.unity.com/packages/slug/163041");
            }
            
            
            EditorGUILayout.BeginVertical();
            {
                GUILayout.FlexibleSpace();
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("© Copyright 2020 Scrollbie Studio", EditorStyles.miniLabel);
                    GUIStyle style = new GUIStyle(EditorStyles.miniLabel);
                    style.alignment = TextAnchor.MiddleRight;
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.LabelField(_version, style, GUILayout.Width(120));
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }

        // private void AddAssemblyDefinitions() {
        //     throw new NotImplementedException();
        // }
    }
}





















