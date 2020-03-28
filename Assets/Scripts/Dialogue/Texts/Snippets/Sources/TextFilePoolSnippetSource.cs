using System.Collections.Generic;
using System.IO;

namespace Assets.Scripts.Dialogue.Texts.Snippets.Sources
{
    public class TextFilePoolSnippetSource : TextFileSnippetSource
    {
        public PoolSnippetSource PoolSource;
        private enum PoolsLoadState { EMPTY, NAME, POOLS };

        public override bool TryGetValue(string name, out object value)
            => PoolSource.TryGetValue(name, out value);

        protected override void LoadSnippets(string text)
        {
            PoolSource.SelectorPools = new Dictionary<string, SelectorPool<object>>();
            using (StringReader textReader = new StringReader(text))
            {
                string line, name = "";
                PoolsLoadState state = PoolsLoadState.EMPTY;
                SelectorPool<object> selectorPool = null;
                while ((line = textReader.ReadLine()) != null)
                {
                    switch (state)
                    {
                        case PoolsLoadState.EMPTY:
                            if (selectorPool == null) { selectorPool = new SelectorPool<object>(); }
                            if (!string.IsNullOrEmpty(line)) { name = line.Trim(); state = PoolsLoadState.NAME; }
                            break;
                        case PoolsLoadState.NAME:
                            if (line == PoolSource.NameValuesSeparator) { state = PoolsLoadState.POOLS; }
                            break;
                        case PoolsLoadState.POOLS:
                            if (line == PoolSource.PoolSeparator)
                            {
                                PoolSource.SelectorPools.Add(name, selectorPool);
                                state = PoolsLoadState.EMPTY;
                                selectorPool = null;
                            }
                            else
                            {
                                selectorPool.Pool.Add(line);
                            }
                            break;
                    }
                }
            }
        }
    }
}
