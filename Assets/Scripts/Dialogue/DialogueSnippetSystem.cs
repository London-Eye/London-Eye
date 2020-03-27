using Assets.Scripts.Dialogue.Texts;
using Assets.Scripts.Dialogue.Texts.Snippets;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Dialogue
{
    public abstract class DialogueSnippetSystem : MonoBehaviour
    {
        public abstract string ParseAndReplace(string text, Action<ParsingException> logger = null);
    }

    public class DialogueSnippetSystem<T> : DialogueSnippetSystem
    {
        public const string DefaultSeparator = "%";

        public string StartSeparator = DefaultSeparator;
        public string EndSeparator = DefaultSeparator;

        protected readonly Dictionary<string, T> snippets = new Dictionary<string, T>();

        public SnippetFormat<T> Format { get; set; }

        protected virtual void Awake()
        {
            Format = new SnippetFormat<T>(StartSeparator, EndSeparator)
            {
                Snippets = snippets
            };
        }

        public override string ParseAndReplace(string text, Action<ParsingException> logger = null)
            => ReplaceSnippets(text, ParseSnippets(text, logger));

        public string ReplaceSnippets(string text, IEnumerable<Snippet<T>> snippets)
        {
            foreach (Snippet<T> snippet in snippets)
            {
                // Replace only the first occurrence of the snippet (to allow different snippets being called the same)
                int indexOfSnippet = text.IndexOf(snippet.FullName);
                if (indexOfSnippet >= 0)
                {
                    text = text.Substring(0, indexOfSnippet)
                        + snippet.Value
                        + text.Substring(indexOfSnippet + snippet.FullName.Length);
                }
            }
            return text;
        }

        public List<Snippet<T>> ParseSnippets(string text, Action<ParsingException> logger = null)
        {
            List<Snippet<T>> result = new List<Snippet<T>>();
            string textBeingAnalyzed = text;

            while (textBeingAnalyzed != null && textBeingAnalyzed.Length > 0)
            {
                // If something went wrong with the snippet, it would skip it
                int indexOfSnippetInit = 0;
                string remainingText = "";
                try
                {
                    Snippet<T> snippet = Format.Extract(textBeingAnalyzed, out indexOfSnippetInit, out _, out remainingText);
                    if (snippet != null)
                    {
                        result.Add(snippet);
                    }
                    else
                    {
                        remainingText = null;
                    }

                    textBeingAnalyzed = remainingText;
                }
                catch (ParsingException ex)
                {
                    // Log the exception
                    logger?.Invoke(ex);
                    Console.WriteLine(ex.Message);

                    // Go to the next portion of the text (Skip the exception source)
                    int nextIndex = (text.Length - textBeingAnalyzed.Length + indexOfSnippetInit) + 1;
                    textBeingAnalyzed = nextIndex >= 0 && nextIndex < textBeingAnalyzed.Length ? textBeingAnalyzed.Substring(nextIndex) : "";
                }
                catch (KeyNotFoundException)
                {
                    // If no value was found for the snippet, just leave it as text
                    textBeingAnalyzed = remainingText;
                }
            }

            return result;
        }
    }
}
