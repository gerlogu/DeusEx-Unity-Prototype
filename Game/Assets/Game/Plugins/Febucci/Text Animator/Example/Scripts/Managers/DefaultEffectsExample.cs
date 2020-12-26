using UnityEngine;

namespace Febucci.UI.Examples
{
    [AddComponentMenu("")]
    public class DefaultEffectsExample : MonoBehaviour
    {
        public TextAnimatorPlayer textAnimatorPlayer;

        private void Awake()
        {
            UnityEngine.Assertions.Assert.IsNotNull(textAnimatorPlayer, $"Text Animator Player component is null in {gameObject.name}");
        }

        private void Start()
        {

            //builds the text with all the default tags
            string builtText = "Built-in effects:\n";
            for (int i = 0; i < TAnimTags.defaultBehaviors.Length; i++)
            {
                builtText += EffectsTesting.AddEffect(TAnimTags.defaultBehaviors[i]);
            }

            //shows the text dynamically (typewriter like)
            textAnimatorPlayer.ShowText(builtText);

        }
    }

}