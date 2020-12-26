using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace Febucci.UI.Core
{
    [System.Serializable]
    public class CharacterEvent : UnityEvent<char> { }

    [DisallowMultipleComponent]
    [RequireComponent(typeof(TextAnimator))]
    public abstract class TAnimPlayerBase : MonoBehaviour
    {
        [System.Flags]
        enum StartTypewriterMode
        {
            /// <summary>
            /// Typewriter starts typing ONLY if you invoke "StartShowingText" from any of your script.
            /// </summary>
            FromScriptOnly = 0,

            /// <summary>
            /// Typewriter automatically starts/resumes from the "OnEnable" method
            /// </summary>
            OnEnable = 1,

            /// <summary>
            /// Typewriter automatically starts once you call "ShowText" method [includes Easy Integration]
            /// </summary>
            OnShowText = 2,

            AutomaticallyFromAllEvents = OnEnable | OnShowText //legacy support for unity 2018.x [instead of automatic recognition in 2019+] 
        }

        #region Variables

        #region Management Variables

        string textToShow = string.Empty;
        TextAnimator _textAnimator;

        /// <summary>
        /// The TextAnimator component linked to this typewriter
        /// </summary>
        public TextAnimator textAnimator
        {
            get
            {
                if (_textAnimator != null)
                    return _textAnimator;

#if UNITY_2019_2_OR_NEWER
                if(!TryGetComponent(out _textAnimator))
                {
                    Debug.LogError($"TextAnimator: Text Animator component is null on GameObject {gameObject.name}");
                }
#else
                _textAnimator = GetComponent<TextAnimator>();
                Assert.IsNotNull(_textAnimator, $"Text Animator component is null on GameObject {gameObject.name}");
#endif

                return _textAnimator;
            }
        }


        /// <summary>
        /// True if the typewriter is currently typing/showing text
        /// </summary>
        protected bool isBaseInsideRoutine => isInsideRoutine;
        bool isInsideRoutine = false;

        protected bool wantsToSkip = false;


        #endregion

        #region Typewriter settings
        [Tooltip("True if you want to shows the text dynamically")]
        [SerializeField] public bool useTypeWriter = true;

        [SerializeField, Tooltip("Controls from which method(s) the typewriter will automatically start/resume. Default is 'Automatic'")]
        StartTypewriterMode startTypewriterMode = StartTypewriterMode.AutomaticallyFromAllEvents;

        #region Typewriter Skip
        [SerializeField]
        bool canSkipTypewriter = true;
        [SerializeField]
        bool hideAppearancesOnSkip = false;
        [SerializeField, Tooltip("True = plays all remaining events once the typewriter has been skipped")]
        bool triggerEventsOnSkip = false;
        #endregion


        /// <summary>
        /// Typewriter's speed (acts like a multiplier)
        /// You can change this value or invoke SetTypewriterSpeed
        /// </summary>
        protected float typewriterPlayerSpeed = 1;


        #endregion

        #endregion

        #region Events
        /// <summary>
        /// Called once the text is completely shown (if typewriter is set to true, this event is called after its end)
        /// </summary>
        public UnityEvent onTextShowed;

        /// <summary>
        /// Called once the typewriter starts showing text.
        /// </summary>
        public UnityEvent onTypewriterStart;

        /// <summary>
        /// Called once a character has been shown
        /// </summary>
        public CharacterEvent onCharacterVisible;

        #endregion

        /// <summary>
        /// Shows remains letters dynamically
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private IEnumerator ShowRemainingCharacters()
        {
            if (!textAnimator.allLettersShown)
            {
                isInsideRoutine = true;
                wantsToSkip = false;

                onTypewriterStart?.Invoke();

                IEnumerator WaitTime(float time)
                {
                    if (time > 0)
                    {
                        float t = 0;
                        while (t <= time)
                        {
                            t += textAnimator.deltaTime;
                            yield return null;
                        }
                    }
                }

                float timeToWait;
                char characterShown;

                typewriterPlayerSpeed = 1;

                float typewriterTagsSpeed = 1;

                bool HasSkipped()
                {
                    return canSkipTypewriter && wantsToSkip;
                }

                float timePassed = 0;

                float deltaTime;
                UpdateDeltaTime();

                void UpdateDeltaTime()
                {

                    deltaTime = textAnimator.deltaTime * typewriterPlayerSpeed * typewriterTagsSpeed;
                }

                //Shows character by character until all are shown
                while (!textAnimator.allLettersShown)
                {
                    //searches for actions [before the character, incl. at the very start of the text]
                    if (textAnimator.hasActions)
                    {
                        //loops until features ended (there could be multiple ones in the same text position, example: when two tags are next to eachother without spaces
                        while (textAnimator.TryGetAction(out TypewriterAction action))
                        {
                            //Default features
                            switch (action.actionID)
                            {
                                case "waitfor":
                                    float waitTime;
                                    FormatUtils.TryGetFloat(action.parameters, 0, 1f, out waitTime);
                                    yield return WaitTime(waitTime);
                                    break;

                                case "waitinput":
                                    yield return WaitInput();
                                    break;

                                case "speed":
                                    FormatUtils.TryGetFloat(action.parameters, 0, 1, out typewriterTagsSpeed);

                                    //clamps speed (time cannot go backwards!)
                                    if (typewriterTagsSpeed <= 0)
                                    {
                                        typewriterTagsSpeed = 0.001f;
                                    }

                                    break;

                                //Action is custom
                                default:
                                    yield return DoCustomAction(action);
                                    break;
                            }

                        }
                    }


                    //increases the visible chars count and stores the new one
                    characterShown = textAnimator.IncreaseVisibleChars();
                    UpdateDeltaTime();

                    //triggers event unless it's a space
                    if (characterShown != ' ')
                    {
                        onCharacterVisible?.Invoke(characterShown);
                    }

                    //gets the time to wait based on the newly character showed
                    timeToWait = WaitTimeOf(characterShown);

                    //waiting less time than a frame, we don't wait yet
                    if (timeToWait < deltaTime)
                    {
                        timePassed += timeToWait;

                        if (timePassed >= deltaTime) //waits only if we "surpassed" a frame duration
                        {
                            yield return null;
                            timePassed %= deltaTime;
                        }
                    }
                    else
                    {
                        while (timePassed < timeToWait && !HasSkipped())
                        {
                            UpdateTypeWriterInput();
                            timePassed += deltaTime;
                            yield return null;
                            UpdateDeltaTime();
                        }

                        timePassed %= timeToWait;
                    }

                    //Skips typewriter
                    if (HasSkipped())
                    {
                        textAnimator.ShowAllCharacters(hideAppearancesOnSkip);

                        if (triggerEventsOnSkip)
                        {
                            textAnimator.TriggerRemainingEvents();
                        }

                        break;
                    }
                }

                // triggers the events at the end of the text
                // 
                // the typewriter is arrived here without skipping
                // meaning that all events were triggered and we only
                // have to fire the ones at the very end
                // (outside tmpro's characters count length)
                if (!canSkipTypewriter || !wantsToSkip)
                {
                    textAnimator.TriggerRemainingEvents();
                }

                isInsideRoutine = false;

                textToShow = string.Empty; //text has been showed, no need to store it now

                onTextShowed?.Invoke();
            }
        }

        #region Public Methods

        public void ShowText(string text)
        {
            StopShowingText();

            if (string.IsNullOrEmpty(text))
            {
                textToShow = string.Empty;
                textAnimator.SyncText(string.Empty, true);
                return;
            }

            textToShow = text;

            wantsToSkip = false;

            textAnimator.SyncText(textToShow, useTypeWriter);

            if (!useTypeWriter)
            {
                onTextShowed?.Invoke();
            }
            else
            {
                if (startTypewriterMode.HasFlag(StartTypewriterMode.OnShowText))
                {
                    StartShowingText();
                }
            }
        }

        #region Typewriter
        /// <summary>
        /// Starts typing [if there are letters left to show]
        /// </summary>
        [ContextMenu("Start Showing Text")]
        public void StartShowingText()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) //prevents from firing in edit mode from the context menu
                return;
#endif

            if(!useTypeWriter)
            {
                Debug.LogWarning("TextAnimator: couldn't start typewriter because 'useTypewriter' is disabled");
                return;
            }

            if (!gameObject.activeInHierarchy)
            {
                Debug.LogWarning("TextAnimator: couldn't start typewriter because the gameobject is not active");
                return;
            }

            if (!isInsideRoutine) //starts only if it is not already typing
            {
                StartCoroutine(ShowRemainingCharacters());
            }
        }


        [ContextMenu("Skip Typewriter")]
        /// <summary>
        /// Skips the typewriter animation (if it's currently showing)
        /// </summary>
        public void SkipTypewriter()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) //prevents from firing in edit mode from the context menu
                return;
#endif
            wantsToSkip = true;
        }


        /// <summary>
        /// Stops typing
        /// </summary>
        [ContextMenu("Stop Showing Text")]
        public void StopShowingText()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) //prevents from firing in edit mode from the context menu
                return;
#endif
            //Stops only if we're inside the routine
            if (isInsideRoutine)
            {
                isInsideRoutine = false;
                StopAllCoroutines();
            }

            textToShow = string.Empty;
        }

        /// <summary>
        /// Sets the internal typewriter speed multiplier.
        /// </summary>
        /// <param name="value"></param>
        public void SetTypewriterSpeed(float value)
        {
            typewriterPlayerSpeed = Mathf.Clamp(value, .001f, value);
        }
        #endregion

        #endregion

        #region Virtual/Abstract Methods
        /// <summary>
        /// Waits for input in order to continue showing text.
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerator WaitInput();

        /// <summary>
        /// Lets us wait for X time based on certain characters
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        protected abstract float WaitTimeOf(char character);


        protected virtual IEnumerator DoCustomAction(TypewriterAction action)
        {
            throw new System.NotImplementedException($"TextAnimator: Custom Action not implemented with type: {action.actionID}. If you did implement it, please do not call the base method from your overridden one.");
        }
        protected virtual void UpdateTypeWriterInput()
        {

        }

        #endregion

        protected virtual void OnDisable()
        {
            isInsideRoutine = false;
        }

        //Resumes/Shows text once the gameobject is active
        protected virtual void OnEnable()
        {
            if (!startTypewriterMode.HasFlag(StartTypewriterMode.OnEnable))
                return;

            StartShowingText();
        }
    }

}