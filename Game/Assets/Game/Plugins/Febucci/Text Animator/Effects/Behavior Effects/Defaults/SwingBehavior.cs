using UnityEngine;

namespace Febucci.UI.Core
{
    class SwingBehavior : BehaviorSine
    {
        public override void SetDefaultValues(BehaviorDefaultValues data)
        {
            amplitude = data.defaults.swingAmplitude;
            frequency = data.defaults.swingFrequency;
            waveSize = data.defaults.swingWaveSize;
        }

        public override void ApplyEffect(ref CharacterData data, int charIndex)
        {
            data.vertices.RotateChar(Mathf.Cos(animatorTime* frequency + charIndex * waveSize) * amplitude);
        }
    }
}