using System.Collections.Generic;
using Yarn.Unity;
using System.Collections.ObjectModel;
using System;

namespace Assets.Scripts.Dialogue
{
    public class VariableStorageYarn : VariableStorageBehaviour
    {
        // Where we actually keep our variables
        private readonly Dictionary<string, Yarn.Value> variables = new Dictionary<string, Yarn.Value>();

        public ReadOnlyDictionary<string, Yarn.Value> Variables => new ReadOnlyDictionary<string, Yarn.Value>(variables);

        public bool IsInitialized { get; private set; }

        public event EventHandler<ReadOnlyDictionary<string, Yarn.Value>> Initialized;

        void Start()
        {
            ResetToDefaults();
        }

        public override void ResetToDefaults()
        {
            IsInitialized = false;
            Clear();

            OnInitialized(Variables);
        }

        protected virtual void OnInitialized(ReadOnlyDictionary<string, Yarn.Value> variables)
        {
            IsInitialized = true;
            Initialized?.Invoke(this, variables);
        }

        public void SetValue<T>(string variableName, T value, bool includeLeading = true)
        {
            Yarn.Value yarnValue = new Yarn.Value(value);

            if (includeLeading)
            {
                variableName = AddLeading(variableName);
            }

            SetValue(variableName, yarnValue: yarnValue);
        }

        private string AddLeading(string variableName) => "$" + variableName;

        public override void SetValue(string variableName, Yarn.Value yarnValue)
        {
            // Copy this value into our list
            variables[variableName] = yarnValue;
        }

        public override Yarn.Value GetValue(string variableName)
            => variables.TryGetValue(variableName, out Yarn.Value value) ? value : Yarn.Value.NULL;

        public bool GetBoolValue(string variableName, bool includeLeading = true)
            => GetObjectValue(variableName, includeLeading).AsBool;

        public string GetStringValue(string variableName, bool includeLeading = true)
            => GetObjectValue(variableName, includeLeading).AsString;

        public float GetNumberValue(string variableName, bool includeLeading = true)
            => GetObjectValue(variableName, includeLeading).AsNumber;

        private Yarn.Value GetObjectValue(string variableName, bool includeLeading = true)
        {
            if (includeLeading)
            {
                variableName = AddLeading(variableName);
            }

            return GetValue(variableName);
        }

        public override void Clear()
        {
            variables.Clear();
        }
    }
}