using UnityEngine;

namespace Febucci.UI.Core
{
    class OffsetAppearance : AppearanceBase
    {
        float amount;
        Vector2 direction;

        public override void SetDefaultValues(AppearanceDefaultValues data)
        {
            direction = data.defaults.offsetDir;
            amount = data.defaults.offsetAmplitude * effectIntensity;
            showDuration = data.defaults.offsetDuration;
        }

        public override void ApplyEffect(ref CharacterData data, int charIndex)
        {
            //Moves all towards a direction
            data.vertices.MoveChar(direction * amount * Tween.EaseIn(1-data.passedTime / showDuration));
        }
    }

}