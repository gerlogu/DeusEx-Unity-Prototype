using UnityEngine;

namespace Febucci.UI.Core
{
    class RotationBehavior : BehaviorBase
    {
        float angleSpeed = 180;
        float angleDiffBetweenChars = 10;

        public override void SetDefaultValues(BehaviorDefaultValues data)
        {
            angleSpeed = data.defaults.angleSpeed;
            angleDiffBetweenChars = data.defaults.angleDiffBetweenChars;
        }

        public override void SetModifier(string modifierName, string modifierValue)
        {
            switch (modifierName)
            {
                //frequency
                case "f": ApplyModifierTo(ref angleSpeed, modifierValue); break;
                //angle diff
                case "s": ApplyModifierTo(ref angleDiffBetweenChars, modifierValue); break;
            }
        }

        public override void ApplyEffect(ref CharacterData data, int charIndex)
        {
            data.vertices.RotateChar(-animatorTime* angleSpeed + angleDiffBetweenChars * charIndex);
        }


        public override string ToString()
        {
            return $"angleSpeed: {angleSpeed}\n" +
                $"angleDiffBetweenChars: {angleDiffBetweenChars}" +
                $"\n{base.ToString()}";
        }

    }

}