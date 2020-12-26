using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;

namespace Febucci.UI.Core
{
    [CustomEditor(typeof(TAnimPlayerBase), true)]
    public class TAnimPlayerBaseDrawer : Editor
    {
        protected SerializedProperty showLettersDinamically { get; private set; }
        SerializedProperty startTypewriterMode;
        SerializedProperty canSkipTypewriter;
        SerializedProperty hideAppearancesOnSkip;
        SerializedProperty triggerEventsOnSkip;

        SerializedProperty onTextShowed;
        SerializedProperty onTypewriterStart;
        SerializedProperty onCharacterVisible;

        string[] propertiesToExclude = new string[0];

        protected virtual string[] GetPropertiesToExclude()
        {
            return new string[] {
            "m_Script",
            "useTypeWriter",
            "startTypewriterMode",
            "canSkipTypewriter",
            "hideAppearancesOnSkip",
            "triggerEventsOnSkip",
            "onTextShowed",
            "onTypewriterStart",
            "onCharacterVisible",
            };
        }

        protected virtual void OnEnable()
        {
            showLettersDinamically = serializedObject.FindProperty("useTypeWriter");
            startTypewriterMode = serializedObject.FindProperty("startTypewriterMode");
            canSkipTypewriter = serializedObject.FindProperty("canSkipTypewriter");
            hideAppearancesOnSkip = serializedObject.FindProperty("hideAppearancesOnSkip");
            triggerEventsOnSkip = serializedObject.FindProperty("triggerEventsOnSkip");


            onTextShowed = serializedObject.FindProperty("onTextShowed");
            onTypewriterStart = serializedObject.FindProperty("onTypewriterStart");
            onCharacterVisible = serializedObject.FindProperty("onCharacterVisible");

            propertiesToExclude = GetPropertiesToExclude();
        }

        public override void OnInspectorGUI()
        {


            {
                EditorGUILayout.LabelField("Main Settings", EditorStyles.boldLabel);

                EditorGUI.indentLevel++;

                EditorGUILayout.PropertyField(showLettersDinamically);

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();

            //Typewriter settings

            EditorGUILayout.LabelField("Typewriter Settings", EditorStyles.boldLabel);
            if (showLettersDinamically.boolValue)
            {

                EditorGUI.indentLevel++;

                EditorGUILayout.PropertyField(startTypewriterMode);

                EditorGUILayout.PropertyField(canSkipTypewriter);

                GUI.enabled = canSkipTypewriter.boolValue;
                EditorGUILayout.PropertyField(hideAppearancesOnSkip);
                EditorGUILayout.PropertyField(triggerEventsOnSkip);
                GUI.enabled = true;

                EditorGUI.indentLevel--;

            }
            else
            {
                GUI.enabled = false;
                EditorGUILayout.LabelField("The typewriter is disabled");
                GUI.enabled = true;
            }

            EditorGUILayout.Space();

            //Events
            {
                EditorGUILayout.LabelField("Events", EditorStyles.boldLabel);

                // foldoutEvents = EditorGUILayout.Foldout(foldoutEvents, "Events");

                //if (foldoutEvents)
                {
                    EditorGUILayout.PropertyField(onTextShowed);

                    //GUI.enabled = showLettersDinamically.boolValue;

                    if (showLettersDinamically.boolValue)
                    {

                        EditorGUILayout.PropertyField(onTypewriterStart);
                        EditorGUILayout.PropertyField(onCharacterVisible);
                    }

                    //GUI.enabled = true;
                }

            }

            //Draws parent without the children (so, TanimPlayerBase can have a custom inspector)
            DrawPropertiesExcluding(serializedObject, propertiesToExclude);

            if (serializedObject.hasModifiedProperties)
            {
                serializedObject.ApplyModifiedProperties();
            }

        }
    }


    [CustomEditor(typeof(TextAnimatorPlayer), true)]
    public class TAnimPlayeDrawer : TAnimPlayerBaseDrawer
    {
        SerializedProperty waitForNormalChars;
        SerializedProperty waitLong;
        SerializedProperty waitMiddle;
        SerializedProperty avoidMultiplePunctuactionWait;

        protected override void OnEnable()
        {
            base.OnEnable();

            waitForNormalChars = serializedObject.FindProperty("waitForNormalChars");
            waitLong = serializedObject.FindProperty("waitLong");
            waitMiddle = serializedObject.FindProperty("waitMiddle");
            avoidMultiplePunctuactionWait = serializedObject.FindProperty("avoidMultiplePunctuactionWait");
        }

        protected override string[] GetPropertiesToExclude()
        {
            string[] newProperties = new string[] {
            "script",
            "waitForNormalChars",
            "waitLong",
            "waitMiddle",
            "avoidMultiplePunctuactionWait",
            };

            string[] baseProperties = base.GetPropertiesToExclude();

            string[] mergedArray = new string[newProperties.Length + baseProperties.Length];

            for (int i = 0; i < baseProperties.Length; i++)
            {
                mergedArray[i] = baseProperties[i];
            }

            for (int i = 0; i < newProperties.Length; i++)
            {
                mergedArray[i + baseProperties.Length] = newProperties[i];
            }

            return mergedArray;
        }

        public override void OnInspectorGUI()
        {

            base.OnInspectorGUI();

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Typewriter Delay", EditorStyles.boldLabel);

            if (showLettersDinamically.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(waitForNormalChars);
                EditorGUILayout.PropertyField(waitLong);
                EditorGUILayout.PropertyField(waitMiddle);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(avoidMultiplePunctuactionWait);
                EditorGUI.indentLevel--;

            }
            else
            {
                GUI.enabled = false;
                EditorGUILayout.LabelField("The typewriter is disabled");
                GUI.enabled = true;
            }


            if (serializedObject.hasModifiedProperties)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}