using UnityEngine;

namespace Assets.Scripts.Dialogue.Texts.Snippets.Sources
{
    public abstract class TextFileSnippetSource : DictionarySnippetSource
    {
        public TextAsset SnippetsFile;

        protected virtual void Awake()
        {
            LoadSnippets(SnippetsFile.text);
        }

        protected abstract void LoadSnippets(string text);

    }
}
