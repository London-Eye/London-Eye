using UnityEngine;
using Yarn;
using Yarn.Unity;

namespace Assets.Scripts.Dialogue.Variables.Storages
{
    public class VariableStorageSelector : VariableStorageGroup
    {
        [Header("Selector")]
        public bool TryOthersOnSelectedFail;

        public int SelectedStorageIndex;

        public void Select(int index) => SelectedStorageIndex = index;
        public void Select(VariableStorageBehaviour variableStorage) => Select(sources.IndexOf(variableStorage));

        protected override Value GetValueBeforeStorage(string variableName)
        {
            if (SelectedStorageIndex >= 0)
            {
                Value value = sources[SelectedStorageIndex].GetValue(variableName);
                return value == Value.NULL && TryOthersOnSelectedFail ? base.GetValueBeforeStorage(variableName) : value; 
            }
            else
            {
                return base.GetValueBeforeStorage(variableName);
            }
        }

        protected override bool SetValueNoStorage(string variableName, Value value)
        {
            if (SelectedStorageIndex >= 0)
            {
                bool succeeded = TrySetValue(sources[SelectedStorageIndex], variableName, value);
                return !succeeded && TryOthersOnSelectedFail ? base.SetValueNoStorage(variableName, value) : succeeded;
            }
            else
            {
                return base.SetValueNoStorage(variableName, value);
            }
        }
    }
}
