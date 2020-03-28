using System.Collections.Generic;

namespace Assets.Scripts.Dialogue.Texts.Snippets.Sources
{
    public class SimpleSnippetSource : DictionarySnippetSource
    {
        public List<string> Names;
        public List<string> Values;

        void Awake()
        {
            for (int i = 0; i < Names.Count; i++)
            {
                Snippets[Names[i]] = Values[i];
            }
        }
    }
}
