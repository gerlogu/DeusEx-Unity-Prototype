using UnityEngine;


namespace Febucci.UI.Core
{
    [System.Serializable]
    public class ColorCurve
    {

        [SerializeField] public bool enabled;

        [SerializeField] protected Gradient gradient;
        [SerializeField, Attributes.MinValue(0.1f)] protected float duration;
        [SerializeField, Range(0, 100)] protected float charsTimeOffset; //clamping to 100 because it repeates the behavior after it

        public float GetDuration()
        {
            return duration;
        }

        bool isAppearance;

        public void Initialize(bool isAppearance)
        {
            this.isAppearance = isAppearance;
            if (duration < .1f)
            {
                duration = .1f;
            }
        }

        public Color32 GetColor(float time, int characterIndex)
        {
            if (isAppearance)
                return gradient.Evaluate(Mathf.Clamp01(time / duration));

            return gradient.Evaluate(((time / duration) % 1f + characterIndex * (charsTimeOffset / 100f)) % 1f);
        }
    }
}