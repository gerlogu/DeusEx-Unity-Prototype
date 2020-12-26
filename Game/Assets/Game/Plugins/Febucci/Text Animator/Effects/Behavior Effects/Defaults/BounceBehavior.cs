using UnityEngine;

namespace Febucci.UI.Core
{
    class BounceBehavior : BehaviorSine
    {
        public override void SetDefaultValues(BehaviorDefaultValues data)
        {
            amplitude = data.defaults.bounceAmplitude;
            frequency = data.defaults.bounceFrequency;
            waveSize = data.defaults.bounceWaveSize;
        }

        public override void ApplyEffect(ref CharacterData data, int charIndex)
        {

            //Calculates the tween percentage
            float BounceTween(float t)
            {
                const float stillTime = .2f;
                const float easeIn = .2f;
                const float bounce = 1 - stillTime - easeIn;

                if (t <= easeIn)
                    return Tween.EaseInOut(t / easeIn);
                t -= easeIn;

                if (t <= bounce)
                    return 1 - Tween.BounceOut(t / bounce);

                return 0;
            }

            data.vertices.MoveChar(Vector3.up * effectIntensity * BounceTween((Mathf.Repeat(animatorTime* frequency - waveSize * charIndex, 1))) * amplitude);
        }
    }

}