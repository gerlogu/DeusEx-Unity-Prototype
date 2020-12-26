using UnityEngine;

namespace Febucci.UI.Core
{
    class SizeBehavior : BehaviorSine
    {
        Vector3 middle;

        public override void SetDefaultValues(BehaviorDefaultValues data)
        {
            amplitude = data.defaults.sizeAmplitude * -1 + 1;
            frequency = data.defaults.sizeFrequency;
            waveSize = data.defaults.sizeWaveSize;
        }

        public override void ApplyEffect(ref CharacterData data, int charIndex)
        {
            middle = data.vertices.GetMiddlePos();

            data.vertices.LerpUnclamped(middle, (Mathf.Cos(animatorTime* frequency + charIndex * waveSize) + 1) / 2f * amplitude);
        }
    }
}