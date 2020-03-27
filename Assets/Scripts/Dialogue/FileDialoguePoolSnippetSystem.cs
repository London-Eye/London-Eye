using Assets.Scripts.Dialogue.Texts.Snippets;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Dialogue
{
    public class FileDialoguePoolSnippetSystem : DialogueSnippetSystem<string>
    {
        public const string DefaultNameValuesSeparator = "---", DefaultPoolSeparator = "===";

        public readonly string NameValuesSeparator = DefaultNameValuesSeparator;

        public readonly string PoolSeparator = DefaultPoolSeparator;

        public TextAsset SnippetPools;

        private enum PoolsLoadState { EMPTY, NAME, POOLS };

        protected override void Awake()
        {
            Format = LoadPools();
        }

        private SelectorPoolSnippetFormat<string> LoadPools()
        {
            SelectorPoolSnippetFormat<string> format = new SelectorPoolSnippetFormat<string>(StartSeparator, EndSeparator);

            using (StringReader textReader = new StringReader(SnippetPools.text))
            {
                string line, name = "";
                PoolsLoadState state = PoolsLoadState.EMPTY;
                SelectorPool<string> selectorPool = null;
                while ((line = textReader.ReadLine()) != null)
                {
                    switch (state)
                    {
                        case PoolsLoadState.EMPTY:
                            if (selectorPool == null) { selectorPool = new SelectorPool<string>(); }
                            if (!string.IsNullOrEmpty(line)) { name = line.Trim(); state = PoolsLoadState.NAME; }
                            break;
                        case PoolsLoadState.NAME:
                            if (line == NameValuesSeparator) { state = PoolsLoadState.POOLS; }
                            break;
                        case PoolsLoadState.POOLS:
                            if (line == PoolSeparator)
                            {
                                format.SelectorPools.Add(name, selectorPool);
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

            return format;
        }
    }
}
