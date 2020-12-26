using UnityEngine;

namespace Febucci.UI.Core
{
    class SlideBehavior : BehaviorSine
    {
        public override void SetDefaultValues(BehaviorDefaultValues data)
        {
            amplitude = data.defaults.slideAmplitude;
            frequency = data.defaults.slideFrequency;
            waveSize = data.defaults.slideWaveSize;
        }

        public override void ApplyEffect(ref CharacterData data, int charIndex)
        {
            float sin = Mathf.Sin(frequency * animatorTime+ charIndex * waveSize) * amplitude * effectIntensity;

            //bottom, torwards one direction
            data.vertices[0] += Vector3.right * sin;
            data.vertices[3] += Vector3.right * sin;
            //top, torwards the opposite dir
            data.vertices[1] += Vector3.right * -sin;
            data.vertices[2] += Vector3.right * -sin;
        }
    }
}