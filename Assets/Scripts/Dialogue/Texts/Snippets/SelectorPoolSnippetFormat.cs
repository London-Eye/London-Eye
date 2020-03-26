using System.Collections.Generic;

namespace Assets.Scripts.Dialogue.Texts.Snippets
{
    public class SelectorPoolSnippetFormat<T> : SnippetFormat<T>
    {
        private const string DefaultNameIDSeparator = "-";

        public Dictionary<string, SelectorPool<T>> SelectorPools { get; }

        public string NameIDSeparator { get; }

        public SelectorPoolSnippetFormat(string startSeparator, string endSeparator, string nameIDSeparator = DefaultNameIDSeparator) : base(startSeparator, endSeparator)
        {
            SelectorPools = new Dictionary<string, SelectorPool<T>>();
            NameIDSeparator = nameIDSeparator;
        }

        public override Snippet<T> CreateSnippet(string fullName)
        {
            int indexOfNameIDSeparator = fullName.IndexOf(NameIDSeparator);
            if (indexOfNameIDSeparator < 0 || !Snippets.ContainsKey(fullName))
            {
                string name = (indexOfNameIDSeparator < 0) ? fullName : fullName.Substring(0, indexOfNameIDSeparator);
                if (SelectorPools.TryGetValue(name, out SelectorPool<T> selectorPool))
                {
                    Snippets[fullName] = selectorPool.Select();
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
            return base.CreateSnippet(fullName);
        }
    }
}
