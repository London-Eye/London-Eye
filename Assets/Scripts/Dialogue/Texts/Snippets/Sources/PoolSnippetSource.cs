using Assets.Scripts.Common;
using System.Collections.Generic;

namespace Assets.Scripts.Dialogue.Texts.Snippets.Sources
{
    public class PoolSnippetSource : DictionarySnippetSource
    {
        public const string DefaultNameIDSeparator = "-";

        public string NameIDSeparator = DefaultNameIDSeparator;

        public Dictionary<string, SelectorPool<object>> SelectorPools { get; set; }
            = new Dictionary<string, SelectorPool<object>>();

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
