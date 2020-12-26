using UnityEngine;

namespace Febucci.UI.Core
{
    public abstract class BehaviorSine : BehaviorBase
    {
        protected float amplitude = 1;
        protected float frequency = 1;
        protected float waveSize = .08f;

        public override void SetModifier(string modifierName, string modifierValue)
        {
            switch (modifierName)
            {
                //amplitude
                case "a": ApplyModifierTo(ref amplitude, modifierValue); break;
                //frequency
                case "f": ApplyModifierTo(ref frequency, modifierValue); break;
                //wave size
                case "w": ApplyModifierTo(ref waveSize, modifierValue); break;
            }
        }

        public override string ToString()
        {
            return $"freq: {frequency}\n" +
                $"ampl: {amplitude}\n" +
                $"waveSize: {waveSize}" +
                $"\n{base.ToString()}";
        }
    }
}