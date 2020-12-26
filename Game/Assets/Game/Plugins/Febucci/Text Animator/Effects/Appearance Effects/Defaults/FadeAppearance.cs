using UnityEngine;

namespace Febucci.UI.Core
{
    class FadeAppearance : AppearanceBase
    {

        public override void SetDefaultValues(AppearanceDefaultValues data)
        {
            showDuration = data.defaults.fadeDuration;
        }

        public override void ApplyEffect(ref CharacterData data, int charIndex)
        {
            //from transparent to real color
            data.colors.LerpUnclamped(Color.clear, Tween.EaseInOut(1 - (data.passedTime / showDuration)));
        }
    }

}