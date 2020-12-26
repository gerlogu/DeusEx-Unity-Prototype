using UnityEngine;

namespace Febucci.UI.Core
{
    class DiagonalExpandAppearance : AppearanceBase
    {
        int targetA;
        int targetB;
        public override void SetDefaultValues(AppearanceDefaultValues data)
        {
            showDuration = data.defaults.diagonalExpandDuration;

            if (data.defaults.diagonalFromBttmLeft) //expands bottom left and top right
            {
                targetA = 0;
                targetB = 2;
            }
            else //expands bottom right and top left
            {
                targetA = 1;
                targetB = 3;
            }
        }

        public override void ApplyEffect(ref CharacterData data, int charIndex)
        {
            Vector2 middlePos = data.vertices.GetMiddlePos();
            float pct = Tween.EaseInOut(data.passedTime / showDuration);
            
            data.vertices[targetA] = Vector3.LerpUnclamped(middlePos, data.vertices[targetA], pct);
            //top right copies from bottom right
            data.vertices[targetB] = Vector3.LerpUnclamped(middlePos, data.vertices[targetB], pct);
        }

    }

}