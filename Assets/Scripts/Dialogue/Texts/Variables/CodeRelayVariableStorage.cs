using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

namespace Assets.Scripts.Dialogue.Texts.Variables
{
    public class CodeRelayVariableStorage : VariableStorageDecorator<VariableStorageBehaviour>
    {
        public bool persistStorage = true;

        [System.Serializable]
        private class CodeRelayUnityEvent : UnityEvent<CodeRelayVariableStorage> { }

        [SerializeField] private CodeRelayUnityEvent StorageBind;

        private VariableStorageBehaviour storageToBind;

        public void BindStorage(VariableStorageBehaviour storage)
        {
            storageToBind = storage;
        }

        protected override VariableStorageBehaviour InitStorage()
        {
            StorageBind.Invoke(this);
            return storageToBind;
        }

        public override void ResetToDefaults()
        {
            ResetToDefaultsBeforeStorage();
            if (!persistStorage) Storage.ResetToDefaults();
            ResetToDefaultsAfterStorage();
        }
    }
}