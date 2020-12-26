using UnityEngine;
using System.Collections.Generic;
using System;

namespace Febucci.UI
{

    public static class TAnimTags
    {

        #region Tags
        public const string bh_Shake = "shake";
        public const string bh_Rot = "rot";
        public const string bh_Wiggle = "wiggle";
        public const string bh_Wave = "wave";
        public const string bh_Swing = "swing";
        public const string bh_Incr = "incr";
        public const string bh_Slide = "slide";
        public const string bh_Bounce = "bounce";
        public const string bh_Fade = "fade";
        public const string bh_Rainb = "rainb";

        public const string ap_Size = "size";
        public const string ap_Fade = "fade";
        public const string ap_Offset = "offset";
        public const string ap_VertExp = "vertexp";
        public const string ap_HoriExp = "horiexp";
        public const string ap_DiagExp = "diagexp";
        public const string ap_Rot = "rot";
        #endregion


        public static readonly string[] defaultBehaviors = new string[]
        {
            bh_Shake,
            bh_Rot,
            bh_Wiggle,
            bh_Wave,
            bh_Swing,
            bh_Incr,
            bh_Slide,
            bh_Bounce,
            bh_Fade,
            bh_Rainb,
        };

        public static readonly string[] defaultAppearances = new string[]{
            ap_Size,
            ap_Fade,
            ap_Offset,
            ap_VertExp,
            ap_HoriExp,
            ap_DiagExp,
            ap_Rot,
        };


    }

}

namespace Febucci.UI.Core
{
#if UNITY_EDITOR
    public static class TAnim_EditorHelper
    {
        internal delegate void VoidCallback();
        internal static event VoidCallback onChangesApplied;

        public static void TriggerEvent()
        {
            if (Application.isPlaying)
            {
                onChangesApplied?.Invoke();
            }
        }
    }
#endif


    internal static class TAnimBuilder
    {
        static TAnimGlobalDataScriptable _data;
        public static bool hasData { get; private set; }
        internal static TAnimGlobalDataScriptable data
        {
            get => _data;
        }


        #region Static Controller

        static Dictionary<string, Type> behaviorsData = new Dictionary<string, Type>();
        static Dictionary<string, Type> appearancesData = new Dictionary<string, Type>();

        static HashSet<string> globalDefaultActions = new HashSet<string>();
        static HashSet<string> globalCustomActions = new HashSet<string>();

        static bool globalDatabaseInitialized;


        internal static void InitializeGlobalDatabase()
        {
            if (globalDatabaseInitialized)
                return;

            globalDatabaseInitialized = true;


            #region Local Methods
            void AddCustomTags<T>(CustomEffects.TagInfo[] tagsArray, ref Dictionary<string, Type> dict) where T : EffectsBase
            {
                if (tagsArray != null && tagsArray.Length > 0)
                {
                    string customKey;
                    for (int i = 0; i < tagsArray.Length; i++)
                    {
                        customKey = tagsArray[i].key;

                        //error, custom key is the same as a built-in one
                        if (dict.ContainsKey(customKey))
                        {
                            continue;
                        }

                        //checks if tag inherits from IEffectsBase
                        if (tagsArray[i].type.IsSubclassOf(typeof(T)))
                        {
                            //adds custom tag
                            dict.Add(customKey, tagsArray[i].type);
                        }
                    }
                }
            }

            #endregion


            #region Behaviors
            behaviorsData.Add(TAnimTags.bh_Shake, typeof(ShakeBehavior));
            behaviorsData.Add(TAnimTags.bh_Rot, typeof(RotationBehavior));
            behaviorsData.Add(TAnimTags.bh_Wiggle, typeof(WiggleBehavior));
            behaviorsData.Add(TAnimTags.bh_Wave, typeof(WaveBehavior));
            behaviorsData.Add(TAnimTags.bh_Swing, typeof(SwingBehavior));
            behaviorsData.Add(TAnimTags.bh_Incr, typeof(SizeBehavior));
            behaviorsData.Add(TAnimTags.bh_Slide, typeof(SlideBehavior));
            behaviorsData.Add(TAnimTags.bh_Bounce, typeof(BounceBehavior));

            behaviorsData.Add(TAnimTags.bh_Fade, typeof(FadeBehavior));
            behaviorsData.Add(TAnimTags.bh_Rainb, typeof(RainbowBehavior));
            #endregion

            #region Appearances
            appearancesData.Add("default", typeof(SizeAppearance));
            appearancesData.Add(TAnimTags.ap_Size, typeof(SizeAppearance));
            appearancesData.Add(TAnimTags.ap_Fade, typeof(FadeAppearance));
            appearancesData.Add(TAnimTags.ap_Offset, typeof(OffsetAppearance));
            appearancesData.Add(TAnimTags.ap_VertExp, typeof(VerticalExpandAppearance));
            appearancesData.Add(TAnimTags.ap_HoriExp, typeof(HorizontalExpandAppearance));
            appearancesData.Add(TAnimTags.ap_DiagExp, typeof(DiagonalExpandAppearance));
            appearancesData.Add(TAnimTags.ap_Rot, typeof(RotatingAppearance));
            #endregion


            hasData = false;
            _data = Resources.Load(TAnimGlobalDataScriptable.resourcesPath) as TAnimGlobalDataScriptable;

            if (data != null)
            {
                hasData = true;

                #region Global Effects
                //Adds global effects
                for (int i = 0; i < data.globalBehaviorPresets.Length; i++)
                {
                    TryAddingPresetToDictionary(ref behaviorsData, data.globalBehaviorPresets[i].effectTag, typeof(PresetBehavior));
                }

                AddCustomTags<BehaviorBase>(CustomEffects.customBehaviors, ref behaviorsData);

                //Adds global effects
                for (int i = 0; i < data.globalAppearancePresets.Length; i++)
                {
                    TryAddingPresetToDictionary(ref appearancesData, data.globalAppearancePresets[i].effectTag, typeof(PresetAppearance));
                }

                AddCustomTags<AppearanceBase>(CustomEffects.customAppearances, ref appearancesData);
                #endregion


                #region Custom Features

                if (data.customActions != null && data.customActions.Length > 0)
                {
                    for (int i = 0; i < data.customActions.Length; i++)
                    {
                        if (data.customActions[i].Length <= 0)
                        {
                            Debug.LogError($"TextAnimator: Custom action {i} has an empty tag!");
                            continue;
                        }

                        if (globalCustomActions.Contains(data.customActions[i]))
                        {
                            Debug.LogError($"TextAnimator: Custom feature with tag '{data.customActions[i]}' is already present, it won't be added to the database.");
                            continue;
                        }

                        globalCustomActions.Add(data.customActions[i]);
                    }
                }

                #endregion
            }

            #region Default Features

            globalDefaultActions.Add("waitfor");
            globalDefaultActions.Add("waitinput");
            globalDefaultActions.Add("speed");

            #endregion
        }

        internal static bool TryGetGlobalPresetBehavior(string tag, out PresetBehaviorValues result)
        {
            if (!hasData) //avoids searching if data is null
            {
                result = default;
                return false;
            }

            return GetPresetFromArray(tag, data.globalBehaviorPresets, out result);
        }

        internal static bool TryGetGlobalPresetAppearance(string tag, out PresetAppearanceValues result)
        {
            if (!hasData)  //avoids searching if data is null
            {
                result = default;
                return false;
            }

            return GetPresetFromArray(tag, data.globalAppearancePresets, out result);
        }

        internal static bool GetPresetFromArray<T>(string tag, T[] presets, out T result) where T : PresetBaseValues
        {
            if (presets.Length > 0)
            {
                for (int i = 0; i < presets.Length; i++)
                {
                    if (tag.Equals(presets[i].effectTag))
                    {
                        result = presets[i];
                        return true;
                    }
                }
            }

            result = default;
            return false;
        }


        internal static bool IsDefaultAction(string tag)
        {
            if (globalDefaultActions.Count > 0 && globalDefaultActions.Contains(tag))
            {
                return true;
            }

            return false;
        }

        internal static bool IsCustomAction(string tag)
        {
            if (globalCustomActions.Count > 0 && globalCustomActions.Contains(tag))
            {
                return true;
            }

            return false;
        }

        internal static bool TryGetGlobalBehaviorFromTag(string tag, out BehaviorBase effectClass)
        {
            return TryGetEffectClassFromTag<BehaviorBase>(behaviorsData, tag, out effectClass);
        }

        internal static bool TryGetGlobalAppearanceFromTag(string tag, out AppearanceBase effectClass)
        {
            return TryGetEffectClassFromTag(appearancesData, tag, out effectClass);
        }

        internal static bool TryGetEffectClassFromTag<T>(Dictionary<string, Type> dictionary, string tag, out T effectClass) where T : class
        {
            //Tag is built-in
            if (dictionary.ContainsKey(tag))
            {
                effectClass = Activator.CreateInstance(dictionary[tag]) as T;
                return true;
            }

            effectClass = default;
            return false;
        }

        internal static void TryAddingPresetToDictionary(ref Dictionary<string, Type> database, string tag, Type type)
        {

            if (string.IsNullOrEmpty(tag))
            {
                Debug.LogWarning($"TextAnimator: Preset has a null or empty tag");
                return;
            }

            if (!TextAnimator.IsTagLongEnough(tag))
            {
                Debug.LogWarning($"TextAnimator: Preset has a tag shorter than three characters.");
                return;
            }

            if (database.ContainsKey(tag))
            {
                Debug.LogWarning($"TextAnimator: A Preset has a tag that's already present, it won't be added to the database.");
                return;
            }

            database.Add(tag, type);
        }


        #endregion

    }

    [System.Serializable]
    [CreateAssetMenu(fileName = "TextAnimator GlobalData", menuName = "TextAnimator/Create Global Text Animator Data")]
    public class TAnimGlobalDataScriptable : ScriptableObject
    {
        /// <summary>
        /// Resources Path where the scriptable object must be stored
        /// </summary>
        public const string resourcesPath = "TextAnimator GlobalData";

        public PresetBehaviorValues[] globalBehaviorPresets = new PresetBehaviorValues[0];
        public PresetAppearanceValues[] globalAppearancePresets = new PresetAppearanceValues[0];

        public string[] customActions = new string[0];
    }

}