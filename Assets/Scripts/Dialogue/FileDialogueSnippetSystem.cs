using Assets.Scripts.Dialogue.Texts.Snippets;
using UnityEngine;

namespace Assets.Scripts.Dialogue
{
    public class FileDialogueSnippetSystem : DialogueSnippetSystem<string>
    {
        public const string DefaultNameValueSeparator = "===";

        public string NameValueSeparator = DefaultNameValueSeparator;

        public TextAsset Snippets;

        protected override void Awake()
        {
            FileSnippetFormat fileFormat = new FileSnippetFormat(StartSeparator, EndSeparator, NameValueSeparator);
            fileFormat.LoadSnippets(Snippets.text);
            Format = fileFormat;
        }
    }
}
