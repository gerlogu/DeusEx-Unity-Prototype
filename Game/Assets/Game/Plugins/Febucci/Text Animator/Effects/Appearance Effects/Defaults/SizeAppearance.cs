namespace Febucci.UI.Core
{
    class SizeAppearance : AppearanceBase
    {
        float amplitude;
        public override void SetDefaultValues(AppearanceDefaultValues data)
        {
            showDuration = data.defaults.sizeDuration;
            amplitude = data.defaults.sizeAmplitude * -1 + 1;
        }

        public override void ApplyEffect(ref CharacterData data, int charIndex)
        {
            data.vertices.LerpUnclamped(
                data.vertices.GetMiddlePos(),
                Tween.EaseIn(1 - (data.passedTime / showDuration)) * amplitude
                );
        }
    }
}