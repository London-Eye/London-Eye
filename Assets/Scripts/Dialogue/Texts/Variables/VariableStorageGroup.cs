using System;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;

namespace Assets.Scripts.Dialogue.Texts.Variables
{
    public class VariableStorageGroup : VariableStorageDecorator<InMemoryVariableStorage>
    {
        public const string ReadOnlyVariableStorageMessage = "This variable storage is read-only";

        public List<VariableStorageBehaviour> sources;

        [Header("Optional debugging options")]
        public bool logSetKnownErrors = true;

        protected override Value GetValueBeforeStorage(string variableName)
        {
            foreach (var source in sources)
            {
                Value value = source.GetValue(variableName);
                if (value != Value.NULL) return value;
            }
            return Value.NULL;
        }

        protected override void ResetToDefaultsBeforeStorage()
        {
            if (sources != null)
            {
                foreach (var source in sources)
                {
                    source.ResetToDefaults();
                }
            }
        }

        protected override bool SetValueNoStorage(string variableName, Value value)
        {
            foreach (var source in sources)
            {
                try
                {
                    source.SetValue(variableName, value);
                    return true;
                }
                catch (SystemException ex) when (ex is ArgumentException || ex is InvalidOperationException)
                {
                    // The source was not compatible with the value. Try another.

                    // Log the exception if the flag is set to true
                    if (logSetKnownErrors)
                    {
                        Debug.LogException(ex);
                    }
                }
            }

            return false;
        }
    }
}