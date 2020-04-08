using Assets.Scripts.Common;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Yarn;

namespace Assets.Scripts.Dialogue.Texts.Variables
{
    public class TextAssetPoolVariableStorage : VariableStorageWithFallback<PoolVariableStorage>
    {
        public TextAsset textAsset;

        public const string DefaultNameValuesSeparator = "---", DefaultPoolSeparator = "===";

        public string NameValuesSeparator = DefaultNameValuesSeparator;

        public string PoolSeparator = DefaultPoolSeparator;

        private enum PoolsLoadState { EMPTY, NAME, POOLS };

        protected override bool SetValueNoFallback(string variableName, Value value)
        {
            throw new System.InvalidOperationException(VariableStorageGroup.ReadOnlyVariableStorageMessage);
        }

        protected override void ResetToDefaultsAfterFallback()
        {
            Fallback.SelectorPools = new Dictionary<string, SelectorPool<object>>();
            using (StringReader textReader = new StringReader(textAsset.text))
            {
                string line, name = "";
                PoolsLoadState state = PoolsLoadState.EMPTY;
                SelectorPool<object> selectorPool = null;
                while ((line = textReader.ReadLine()) != null)
                {
                    switch (state)
                    {
                        case PoolsLoadState.EMPTY:
                            if (selectorPool == null)
                            {
                                selectorPool = new SelectorPool<object>();
                            }
                            if (!string.IsNullOrEmpty(line))
                            {
                                name = line.Trim();
                                state = PoolsLoadState.NAME;
                            }
                            break;
                        case PoolsLoadState.NAME:
                            if (line == NameValuesSeparator) { state = PoolsLoadState.POOLS; }
                            break;
                        case PoolsLoadState.POOLS:
                            if (line == PoolSeparator)
                            {
                                Fallback.SelectorPools.Add(name, selectorPool);
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
