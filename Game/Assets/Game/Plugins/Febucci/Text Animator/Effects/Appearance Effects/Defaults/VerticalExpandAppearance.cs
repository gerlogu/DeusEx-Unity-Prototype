using UnityEngine;

namespace Febucci.UI.Core
{
    class VerticalExpandAppearance : AppearanceBase
    {
        int startA, targetA;
        int startB, targetB;
        public override void SetDefaultValues(AppearanceDefaultValues data)
        {
            showDuration = data.defaults.verticalExpandDuration;

            if (data.defaults.verticalFromBottom) //From bottom to top 
            {

                //top left copies bottom left
                startA = 0;
                targetA = 1;

                //top right copies bottom right
                startB = 3;
                targetB = 2;
            }
            else //from top to bottom
            {

                //bottom left copies top left
                startA = 1;
                targetA = 0;

                //bottom right copies top right
                startB = 2;
                targetB = 3;

            }
        }

        public override void ApplyEffect(ref CharacterData data, int charIndex)
        {
            float pct = Tween.EaseInOut(data.passedTime / showDuration);
            
            data.vertices[targetA] = Vector3.LerpUnclamped(data.vertices[startA], data.vertices[targetA], pct);
            data.vertices[targetB] = Vector3.LerpUnclamped(data.vertices[startB], data.vertices[targetB], pct);
        }

    }

}