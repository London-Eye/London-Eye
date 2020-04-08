using Assets.Scripts.Common;
using System.Collections.Generic;
using Yarn;
using Yarn.Unity;

namespace Assets.Scripts.Dialogue.Texts.Variables
{
    public class PoolVariableStorage : VariableStorageWithFallback<InMemoryVariableStorage>
    {
        public const string DefaultNameIDSeparator = "-";

        public string NameIDSeparator = DefaultNameIDSeparator;

        public Dictionary<string, SelectorPool<object>> SelectorPools { get; set; }
            = new Dictionary<string, SelectorPool<object>>();


        protected override Value GetValueAfterFallback(string name)
        {
            int indexOfNameIDSeparator = name.IndexOf(NameIDSeparator);
            bool hasID = indexOfNameIDSeparator >= 0;

            string poolName = hasID ? name.Substring(0, indexOfNameIDSeparator) : name;
            if (SelectorPools.TryGetValue(poolName, out SelectorPool<object> pool))
            {
                object obj = pool.Select();
                Value value;
                if (obj is Value) value = obj as Value;
                else value = new Value(obj);

                if (hasID) Fallback.SetValue(name, value);
                return value;
            }
            else
            {
                return null;
            }
        }

        protected override bool SetValueNoFallback(string variableName, Value value)
        {
            throw new System.InvalidOperationException(VariableStorageGroup.ReadOnlyVariableStorageMessage);
        }
    }
}
