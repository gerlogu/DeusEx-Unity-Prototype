using System;

namespace Febucci.UI.Core
{
    /// <summary>
    /// 'Top' class that lets you add custom effects
    /// </summary>
    public static class CustomEffects
    {
        public struct TagInfo
        {
            /// <summary>
            /// lowered tags ID
            /// </summary>
            public readonly string key;
            public readonly Type type;

            public TagInfo(string key, Type type)
            {
                this.key = key.ToLower();
                this.type = type;
            }
        }

        #region Behaviors

        public readonly static TagInfo[] customBehaviors = new TagInfo[]
        {
            //new TagInfo("yourTagID", typeof(SizeBehavior)), //tag ID, tags' class (has to inherit BehaviorBase)
        };

        [Serializable]
        public class CustomBehaviorDefValues
        {
            //write here the parameters of your custom effects

            //example: public float shrinkAmplitude = 123;
        }

        #endregion

        #region Appearances

        public readonly static TagInfo[] customAppearances = new TagInfo[]
        {
            //new TagInfo("yourTagID", typeof(SizeAppearance)), //tag ID, tags' class (has to inherit to AppearanceBase)
        };

        [Serializable]
        public class CustomAppearanceDefValues
        {
            //write here the parameters of your custom effects

            //example: public float shrinkAmplitude = 123;
        }

        #endregion


    }

}