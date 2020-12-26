using System.Collections;
using UnityEngine;

namespace Febucci.UI
{

    [HelpURL("https://www.textanimator.febucci.com/docs/text-animator-players/")]
    [AddComponentMenu("Febucci/TextAnimator/TextAnimatorPlayer")]
    public class TextAnimatorPlayer : Core.TAnimPlayerBase
    {
        [SerializeField] [Attributes.CharsDisplayTime] [Tooltip("Wait time for normal letters")] float waitForNormalChars = .03f;
        [SerializeField] [Attributes.CharsDisplayTime] [Tooltip("Wait time for ! ? .")] float waitLong = .6f;
        [SerializeField] [Attributes.CharsDisplayTime] [Tooltip("Wait time for ; : ) - ,")] float waitMiddle = .2f;
        [SerializeField] [Tooltip("-True: only the last punctuaction on a sequence waits for its category time.\n-False: each punctuaction will wait, regardless if it's in a sequence or not")] bool avoidMultiplePunctuactionWait = false;

        protected override float WaitTimeOf(char character)
        {
            //avoids waiting for multiple times if there are puntuactions near each other
            if (avoidMultiplePunctuactionWait && char.IsPunctuation(character))
            {
                if (textAnimator.TryGetNextCharacter(out var result))
                {
                    if (char.IsPunctuation(result.character)) //next character is punctuation
                    {
                        return waitForNormalChars;
                    }
                }
            }

            //character is not before another punctuaction
            switch (character)
            {
                case ';':
                case ':':
                case ')':
                case '-':
                case ',': return waitMiddle;

                case '!':
                case '?':
                case '.':
                    return waitLong;
            }

            return waitForNormalChars;
        }

        protected override IEnumerator WaitInput()
        {
            while (!Input.anyKeyDown)
                yield return null;
        }

    }
}