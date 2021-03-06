﻿using Assets.Scripts.Common;
using Assets.Scripts.Common.Pools;
using System.Collections.Generic;
using Yarn;
using Yarn.Unity;

namespace Assets.Scripts.Dialogue.Variables.Storages
{
    public class PoolVariableStorage : VariableStorageDecorator<InMemoryVariableStorage>
    {
        public Dictionary<string, SelectorPool<object>> SelectorPools { get; set; }
            = new Dictionary<string, SelectorPool<object>>();

        protected override Value GetValueAfterStorage(string name)
        {
            string poolName = this.RemoveLeadingIfPresent(name);
            if (SelectorPools.TryGetValue(poolName, out SelectorPool<object> pool))
            {
                Value value = Utilities.AsYarnValue(pool.Select());
                if (Storage != null) Storage.SetValue(name, value);
                return value;
            }
            else
            {
                return Value.NULL;
            }
        }

        protected override bool SetValueNoStorage(string variableName, Value value)
        {
            throw new System.InvalidOperationException(VariableStorageGroup.ReadOnlyVariableStorageMessage);
        }
    }
}
