using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

namespace Assets.Scripts.Dialogue.Variables.Storages
{
    public class CodeRelayVariableStorage : VariableStorageDecorator<VariableStorageBehaviour>
    {
        [System.Serializable]
        private class CodeRelayUnityEvent : UnityEvent<CodeRelayVariableStorage> { }

        [SerializeField] private CodeRelayUnityEvent OnInitStorage;

        private VariableStorageBehaviour storageToBind;

        public void BindStorage(VariableStorageBehaviour storage)
        {
            storageToBind = storage;
        }

        protected override VariableStorageBehaviour InitStorage()
        {
            OnInitStorage.Invoke(this);
            return storageToBind;
        }

        public override void ResetToDefaults()
        {
            if (Storage == storageToBind)
            {
                base.ResetToDefaults();
            }
            else
            {
                ResetToDefaults(storageToBind);
            }
        }
    }
}