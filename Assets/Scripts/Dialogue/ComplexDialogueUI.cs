using Assets.Scripts.Dialogue.Texts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace Assets.Scripts.Dialogue
{
    public class ComplexDialogueUI : DialogueUI
    {
        private DialogueSnippetSystem[] snippetSystems;
        private VariableSnippetSystem variableSystem;

        // When true, the user has indicated that they want to proceed to
        // the next line.
        private bool proceedToNextLine = false;

        void Start()
        {
            snippetSystems = FindObjectsOfType<DialogueSnippetSystem>();
            variableSystem = FindObjectOfType<VariableSnippetSystem>();
        }

        public override Yarn.Dialogue.HandlerExecutionType RunLine(Yarn.Line line, IDictionary<string, string> strings, System.Action onComplete)
        {
            // Start displaying the line; it will call onComplete later
            // which will tell the dialogue to continue
            StartCoroutine(DoRunLine(line, strings, onComplete));
            return Yarn.Dialogue.HandlerExecutionType.PauseExecution;
        }

        /// Show a line of dialogue, gradually        
        private IEnumerator DoRunLine(Yarn.Line line, IDictionary<string, string> strings, System.Action onComplete)
        {
            onLineStart?.Invoke();

            proceedToNextLine = false;

            if (strings.TryGetValue(line.ID, out var text))
            {
                // Replace snippets with real text
                if (snippetSystems != null && snippetSystems.Length > 0)
                {
                    foreach (var snippetSystem in snippetSystems)
                    {
                        text = snippetSystem.ParseAndReplace(text, RunLineLogger);
                    }
                }

                // Replace variables with real text
                if (variableSystem != null)
                {
                    text = variableSystem.ParseAndReplace(text, RunLineLogger);
                }
            }
            else
            {
                Debug.LogWarning($"Line {line.ID} doesn't have any localised text.");
                text = line.ID;
            }

            if (textSpeed > 0.0f)
            {
                IDialogueText completeText = ComplexDialogueText.AnalyzeText(text, RunLineLogger);

                foreach (string currentText in completeText.Parse())
                {
                    onLineUpdate?.Invoke(currentText);
                    if (proceedToNextLine)
                    {
                        // We've requested a skip of the entire line.
                        // Display all of the text immediately.
                        onLineUpdate?.Invoke(text);
                        break;
                    }
                    yield return new WaitForSeconds(textSpeed);
                }
            }
            else
            {
                // Display the entire line immediately if textSpeed <= 0
                onLineUpdate?.Invoke(text);
            }

            // We're now waiting for the player to move on to the next line
            proceedToNextLine = false;

            // Indicate to the rest of the game that the line has finished being delivered
            onLineFinishDisplaying?.Invoke();

            while (!proceedToNextLine)
            {
                yield return null;
            }

            // Avoid skipping lines if textSpeed == 0
            yield return new WaitForEndOfFrame();

            // Hide the text and prompt
            onLineEnd?.Invoke();

            onComplete();

        }

        public new void MarkLineComplete()
        {
            proceedToNextLine = true;
        }

        private void RunLineLogger(ParsingException parsingException)
        {
            Debug.LogError($"Error: {parsingException.Message}");
        }
    }

}