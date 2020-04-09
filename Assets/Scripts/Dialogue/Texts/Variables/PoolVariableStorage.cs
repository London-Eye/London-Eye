using Assets.Scripts.Common;
using System.Collections.Generic;
using Yarn;
using Yarn.Unity;

namespace Assets.Scripts.Dialogue.Texts.Variables
{
    public class PoolVariableStorage : VariableStorageDecorator<InMemoryVariableStorage>
    {
        public const string DefaultNameIDSeparator = "-";

        public string NameIDSeparator = DefaultNameIDSeparator;

        public Dictionary<string, SelectorPool<object>> SelectorPools { get; set; }
            = new Dictionary<string, SelectorPool<object>>();


        protected override Value GetValueAfterStorage(string name)
        {
            int indexOfNameIDSeparator = name.IndexOf(NameIDSeparator);
            bool hasID = indexOfNameIDSeparator >= 0;

            string poolName = hasID ? name.Substring(0, indexOfNameIDSeparator) : name;
            if (SelectorPools.TryGetValue(poolName, out SelectorPool<object> pool))
            {
                Value value = Utilities.AsYarnValue(pool.Select());

                if (hasID) Storage.SetValue(name, value);
                return value;
            }
            else
            {
                return null;
            }
        }

        protected override bool SetValueNoStorage(string variableName, Value value)
        {
            throw new System.InvalidOperationException(VariableStorageGroup.ReadOnlyVariableStorageMessage);
        }
    }
}
