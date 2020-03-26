using Assets.Scripts.Dialogue.Texts;
using Assets.Scripts.Dialogue.Texts.Snippets;
using System;
using UnityEngine;

namespace Assets.Scripts.Dialogue
{
    public class FileDialogueSnippetSystem : DialogueSnippetSystem<string>
    {
        public const string DefaultNameValueSeparator = "===";

        public string NameValueSeparator = DefaultNameValueSeparator;

        public TextAsset Snippets;

        protected override void Start()
        {
            FileSnippetFormat fileFormat = new FileSnippetFormat(StartSeparator, EndSeparator, NameValueSeparator);
            fileFormat.LoadSnippets(Snippets.text);
            Format = fileFormat;
        }
    }
}
