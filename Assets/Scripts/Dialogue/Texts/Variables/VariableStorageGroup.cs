using Assets.Scripts.Dialogue.Texts.Variables;
using System;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;

public class VariableStorageGroup : VariableStorageWithFallback<InMemoryVariableStorage>
{
    public const string ReadOnlyVariableStorageMessage = "This variable storage is read-only";

    public List<VariableStorageBehaviour> sources;

    public bool logKnownExceptionsOnSet;

    protected override Value GetValueBeforeFallback(string variableName)
    {
        foreach (var source in sources)
        {
            Value value = source.GetValue(variableName);
            if (value != Value.NULL) return value;
        }
        return Value.NULL;
    }

    protected override void ResetToDefaultsBeforeFallback()
    {
        foreach (var source in sources)
        {
            source.ResetToDefaults();
        }
    }

    protected override bool SetValueNoFallback(string variableName, Value value)
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
                if (logKnownExceptionsOnSet)
                {
                    Debug.LogException(ex);
                }
            }
        }

        return false;
    }
}
