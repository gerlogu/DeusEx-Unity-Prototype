using UnityEditor;

namespace MPUIKIT.Editor
{
    public class MPImageShaderGUI : ShaderGUI
    {
        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            EditorGUILayout.HelpBox(
                "Nothing to modify here. Select an MPImage component in the hierarchy and modify the values in the inspector.",
                MessageType.Info);
        }
    }
}