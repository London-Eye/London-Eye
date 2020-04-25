using Assets.Scripts.Dialogue.Texts;
using Assets.Scripts.Dialogue.Variables.Attributes;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

namespace Assets.Scripts.Dialogue
{
    public class ComplexDialogueUI : DialogueUI
    {
        public const char LineStartPlaceHolder = ':';

        // When true, the user has indicated that they want to proceed to
        // the next line.
        private bool proceedToNextLine = false;

        private bool skipDialogue = false;
        private ContinueMode originalContinueMode;

        [Header("Skip Dialogue")]
        [YarnAccess]
        public bool AllowSkip;

        public KeyCode SkipKey = KeyCode.S;

        public KeyCode FastForwardKey = KeyCode.Space;
        private float originalTextSpeed = 0;

        public override Yarn.Dialogue.HandlerExecutionType RunLine(Yarn.Line line, ILineLocalisationProvider localisationProvider, Action onComplete)
        {
            proceedToNextLine = false;

            if (skipDialogue)
            {
                if (AllowSkip)
                {
                    return Yarn.Dialogue.HandlerExecutionType.ContinueExecution;
                }
                else
                {
                    SkipDialogueEnd();
                }
            }

            // Start displaying the line; it will call onComplete later
            // which will tell the dialogue to continue
            StartCoroutine(DoRunLine(line, localisationProvider, onComplete));
            return Yarn.Dialogue.HandlerExecutionType.PauseExecution;
        }

        /// Show a line of dialogue, gradually        
        private IEnumerator DoRunLine(Yarn.Line line, ILineLocalisationProvider localisationProvider, Action onComplete)
        {
            onLineStart?.Invoke();

            // The final text we'll be showing for this line.
            string text = localisationProvider.GetLocalisedTextForLine(line);

            if (text == null)
            {
                Debug.LogWarning($"Line {line.ID} doesn't have any localised text.");
                text = line.ID;
            }
            else if (text[0] == LineStartPlaceHolder)
            {
                text = text.Remove(0, 1);
            }

            if (textSpeed > 0.0f && !proceedToNextLine)
            {
                IDialogueText completeText = ComplexDialogueText.AnalyzeText(text, RunLineLogger);

                foreach (string currentText in completeText.Parse())
                {
                    LineUpdate(currentText);
                    if (proceedToNextLine)
                    {
                        // We've requested a skip of the entire line.
                        // Display all of the text immediately.
                        LineUpdate(text);
                        break;
                    }
                    yield return new WaitForSeconds(textSpeed);
                }
            }
            else
            {
                // Display the entire line immediately if textSpeed <= 0
                LineUpdate(text);
            }

            // We're now waiting for the player to move on to the next line
            proceedToNextLine = false;

            // Indicate to the rest of the game that the line has finished being delivered
            LineFinishDisplaying();

            while (!proceedToNextLine)
            {
                yield return CheckContinue();
            }

            // Avoid skipping lines if textSpeed == 0
            yield return new WaitForEndOfFrame();

            // Hide the text and prompt
            LineEnd();

            onComplete();

        }

        private void LineEnd()
        {
            Input.ResetInputAxes();
            onLineEnd?.Invoke();
        }

        public void LineUpdate(string text)
        {
            CheckSkipButton();

            if (Input.GetKey(ContinueKey))
            {
                MarkLineComplete();
            }

            if (Input.GetKey(FastForwardKey))
            {
                if (originalTextSpeed == 0)
                {
                    originalTextSpeed = textSpeed;
                }

                textSpeed /= 1.2f;
            }

            onLineUpdate?.Invoke(text);
        }

        private bool CheckSkipButton()
        {
            if (!skipDialogue && Input.GetKey(SkipKey))
            {
                if (AllowSkip)
                {
                    SkipDialogue();
                }
                else
                {
                    MarkLineComplete();
                }
                return true;
            }
            return false;
        }

        public new void MarkLineComplete()
        {
            proceedToNextLine = true;
        }

        public override void DialogueComplete()
        {
            if (skipDialogue) SkipDialogueEnd();

            if (originalTextSpeed > 0)
            {
                textSpeed = originalTextSpeed;
                originalTextSpeed = 0;
            }

            base.DialogueComplete();
        }

        public void SkipDialogue()
        {
            skipDialogue = true;

            originalContinueMode = continueMode;
            continueMode = ContinueMode.Skip;

            onOptionsStart.AddListener(SkipDialogueEnd);

            MarkLineComplete();
        }

        private void SkipDialogueEnd()
        {
            if (skipDialogue)
            {
                if (continueMode == ContinueMode.Skip) continueMode = originalContinueMode;
                onOptionsStart.RemoveListener(SkipDialogueEnd);
                skipDialogue = false;
            }
        }

        #region Continue Mode
        public enum ContinueMode { Button, Time, Skip }

        [Header("Continue Modes")]
        public ContinueMode continueMode = ContinueMode.Button;

        [YarnAccess(name = nameof(ContinueMode))]
        public string ContinueModeAsString
        {
            get => continueMode.ToString();
            set => continueMode = (ContinueMode)Enum.Parse(typeof(ContinueMode), value, true);
        }

        public Button ContinueButton;

        public KeyCode ContinueKey = KeyCode.Return;

        [Tooltip("Time between the line end and continuing to the next (only when ContinueMode is set to 'Time'")]
        [YarnAccess]
        public float ContinueTime = 2.5f;

        public void LineFinishDisplaying()
        {
            switch (continueMode)
            {
                case ContinueMode.Button:
                    ContinueButton.gameObject.SetActive(true);
                    break;
            }

            onLineFinishDisplaying?.Invoke();
        }

        private IEnumerator CheckContinue()
        {
            switch (continueMode)
            {
                case ContinueMode.Button:
                    yield return ContinueOnKey(ContinueKey);
                    break;
                case ContinueMode.Time:
                    yield return ContinueAfter(ContinueTime);
                    break;
                case ContinueMode.Skip:
                    MarkLineComplete();
                    break;
            }
        }

        private IEnumerator ContinueOnKey(KeyCode key)
        {
            // Wait for user to release the continue key
            while (Input.GetKey(key))
            {
                yield return null;
            }

            while (!proceedToNextLine)
            {
                // Continue when the user presses the continue key
                if (Input.GetKey(key))
                {
                    MarkLineComplete();
                }
                else if (!CheckSkipButton())
                {
                    yield return new WaitForEndOfFrame();
                }
            }
        }

        private IEnumerator ContinueAfter(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            MarkLineComplete();
        }
        #endregion


        private void RunLineLogger(Exception ex)
        {
            Debug.LogException(ex);
        }
    }

}