using Assets.Scripts.Dialogue.Texts.Snippets;
using Assets.Scripts.Dialogue.Texts.Snippets.Sources;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Dialogue
{
    public class DialogueSnippetSystem : MonoBehaviour
    {
        public const string DefaultSeparator = "%";

        public string StartSeparator = DefaultSeparator;
        public string EndSeparator = DefaultSeparator;

        public SnippetSource Source;

        [Header("Object Access")]
        public bool ObjectAccessEnabled;
        public string StartAccessSeparator = ComplexSnippetFormat<object>.DefaultStartAccessSeparator;
        public string EndAccessSeparator = ComplexSnippetFormat<object>.DefaultEndAccessSeparator;
        public string AccessMemberSeparator = ComplexSnippetFormat<object>.DefaultAccessMemberSeparator;

        public SnippetFormat<object> Format { get; set; }

        void Awake()
        {
            InitFormat();
        }
        
        private void InitFormat()
        {
            if (ObjectAccessEnabled)
            {
                Format = new ComplexSnippetFormat<object>(StartSeparator, EndSeparator, Source);
            }
            else
            {
                Format = new SnippetFormat<object>(StartSeparator, EndSeparator, Source);
            }
        }

        public string ParseAndReplace(string text, Action<Exception> logger = null)
            => ReplaceSnippets(text, ParseSnippets(text, logger));

        public string ReplaceSnippets(string text, IEnumerable<Snippet<object>> snippets)
        {
            foreach (Snippet<object> snippet in snippets)
            {
                // Replace only the first occurrence of the snippet (to allow different snippets being called the same)
                int indexOfSnippet = text.IndexOf(snippet.FullName);
                if (indexOfSnippet >= 0)
                {
                    text = text.Substring(0, indexOfSnippet)
                        + snippet.Value.ToString()
                        + text.Substring(indexOfSnippet + snippet.FullName.Length);
                }
            }
            return text;
        }

        public List<Snippet<object>> ParseSnippets(string text, Action<Exception> logger = null)
        {
            List<Snippet<object>> result = new List<Snippet<object>>();
            string textBeingAnalyzed = text;

            while (textBeingAnalyzed != null && textBeingAnalyzed.Length > 0)
            {
                // If something went wrong with the snippet, it would skip it
                int indexOfSnippetInit = 0;
                string remainingText = "";
                try
                {
                    Snippet<object> snippet = Format.Extract(textBeingAnalyzed, out indexOfSnippetInit, out _, out remainingText);
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
                catch (Exception ex)
                {
                    // Log the exception
                    logger?.Invoke(ex);
                    Console.WriteLine(ex.Message);

                    // Go to the next portion of the text (Skip the exception source)
                    int nextIndex = (text.Length - textBeingAnalyzed.Length + indexOfSnippetInit) + 1;
                    textBeingAnalyzed = nextIndex >= 0 && nextIndex < textBeingAnalyzed.Length ? textBeingAnalyzed.Substring(nextIndex) : "";
                }
            }

            return result;
        }
    }
}
