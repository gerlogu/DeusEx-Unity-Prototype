using UnityEngine;

namespace Febucci.UI.Core.Examples
{
    //Documentation link: https://www.febucci.com/textanimator-docs
    //Example class
    public class BehaviorTemplate : BehaviorBase
    {
        //float amplitude;

        public override void SetDefaultValues(BehaviorDefaultValues data)
        {
            /// Sets the default values of this effect here 

            //example: 
            //amplitude = data.customs.templateAmplitude;
        }

        public override void SetModifier(string modifierName, string modifierValue)
        {
            /*
            switch (modifierName)
            {
                //amplitude
                case "a":
                    ApplyFloatModifier(ref amplitude, modifierValue);
                    break;
                //[...] other modifiers
            }
            */
        }

        public override void ApplyEffect(ref CharacterData data, int charIndex)
        {
            //Take a look at the TextUtilities class.
            //See also how default classes are implemented.

            //Example:
            //Moving a character towards a direction
            //Using the help of the TextUtilities class
            //data.vertices = data.vertices.MoveChar(direction);


            /*
            //Alternative way, moving each vertex manually (same result as the above)
	        for (byte i = 0; i < data.vertices.Length; i++)
	        {
		        data.vertices[i] += dir;
	        }
            */

            /*
             // You can also Modify colors
             for (byte i = 0; i < data.colors.Length; i++)
             {
                data.colors[i] = //change colors
             }
             */
        }

    }
}