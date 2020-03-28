using System.Collections.Generic;

namespace Assets.Scripts.Dialogue.Texts.Snippets.Sources
{
    public class PoolSnippetSource : DictionarySnippetSource
    {
        public const string DefaultNameValuesSeparator = "---", DefaultPoolSeparator = "===", DefaultNameIDSeparator = "-";

        public string NameValuesSeparator = DefaultNameValuesSeparator;

        public string PoolSeparator = DefaultPoolSeparator;

        public string NameIDSeparator = DefaultNameIDSeparator;

        public Dictionary<string, SelectorPool<object>> SelectorPools { get; set; }

        public override bool TryGetValue(string name, out object value)
        {
            int indexOfNameIDSeparator = name.IndexOf(NameIDSeparator);
            bool hasID = indexOfNameIDSeparator >= 0;

            if (!hasID || !base.TryGetValue(name, out value))
            {
                string poolName = hasID ? name.Substring(0, indexOfNameIDSeparator) : name;
                if (SelectorPools.TryGetValue(poolName, out SelectorPool<object> pool))
                {
                    value = pool.Select();
                    if (hasID) Snippets[name] = value;
                    return true;
                }
                else
                {
                    value = null;
                    return false;
                }
            }
            else
            {
                return true; // Has ID, and it found a value
            }
        }
    }
}
