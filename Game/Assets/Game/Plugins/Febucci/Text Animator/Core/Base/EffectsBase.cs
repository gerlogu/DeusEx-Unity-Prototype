namespace Febucci.UI.Core
{
    /// <summary>
    /// Base class for all the effects
    /// </summary>
    public abstract class EffectsBase
    {
        protected float effectIntensity = 1;

        internal string effectTag;
        internal bool closed = false;
        internal int charStartIndex;
        internal int charEndIndex;

        public bool IsCharInsideRegion(int charIndex)
        {
            return charIndex >= charStartIndex && charIndex < charEndIndex;
        }

        public override string ToString()
        {
            return $"{GetType().ToString()}\nStart index: {charStartIndex} - End index: {charEndIndex}";
        }

        public abstract void ApplyEffect(ref CharacterData data, int charIndex);


        protected void ApplyModifierTo(ref float value, string modifierValue)
        { 
            if (FormatUtils.ParseFloat(modifierValue, out float multiplier))
            {
                value *= multiplier;
            }
        }

        internal void SetEffectIntensity(float value)
        {
            effectIntensity = value;
        }

        /// <summary>
        /// Invoked upon effect creation
        /// </summary>
        /// <param name="charactersCount"></param>
        public virtual void Initialize(int charactersCount)
        {

        }

        /// <summary>
        /// Used to calculate effect values (once each frame) before applying it to all the letters
        /// </summary>
        public virtual void Calculate()
        {

        }

    }
}