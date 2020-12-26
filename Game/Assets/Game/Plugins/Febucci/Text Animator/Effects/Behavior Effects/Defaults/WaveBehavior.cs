using UnityEngine;

namespace Febucci.UI.Core
{
    class WaveBehavior : BehaviorSine
    {
        public override void SetDefaultValues(BehaviorDefaultValues data)
        {
            amplitude = data.defaults.waveAmplitude;
            frequency = data.defaults.waveFrequency;
            waveSize = data.defaults.waveWaveSize;
        }

        public override void ApplyEffect(ref CharacterData data, int charIndex)
        {
            data.vertices.MoveChar(Vector3.up * Mathf.Sin(animatorTime* frequency + charIndex * waveSize) * amplitude * effectIntensity);
        }
    }

}