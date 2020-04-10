using Yarn;
using Yarn.Unity;

namespace Assets.Scripts.Dialogue.Variables.Storages
{
    public abstract class VariableStorageDecorator<T> : VariableStorageBehaviour where T : VariableStorageBehaviour
    {
        public bool persistStorage;

        public T Storage { get; private set; }

        /// Reset to our default values when the game starts
        internal void Awake()
        {
            Storage = InitStorage();
            ResetToDefaults();
        }

        protected virtual T InitStorage()
        {
            gameObject.SetActive(false);

            T storage = gameObject.AddComponent<T>();

            // This boilerplate code is needed to avoid InMemoryVariableStorage to throw a NullReferenceException
            // The SetActive(false) and SetActive(true) are needed too in order for this to work
            if (storage is InMemoryVariableStorage memoryStorage)
            {
                memoryStorage.defaultVariables = new InMemoryVariableStorage.DefaultVariable[0];
            }

            gameObject.SetActive(true);

            return storage;
        }

        public override Value GetValue(string variableName)
        {
            Value value = GetValueBeforeStorage(variableName);
            if (value == Value.NULL)
            {
                value = Storage.GetValue(variableName);
                if (value == Value.NULL)
                {
                    value = GetValueAfterStorage(variableName);
                }
            }
            return value;
        }

        protected virtual Value GetValueBeforeStorage(string variableName) => Value.NULL;
        protected virtual Value GetValueAfterStorage(string variableName) => Value.NULL;

        public override void ResetToDefaults()
        {
            ResetToDefaultsBeforeStorage();
            if (!persistStorage) Storage.ResetToDefaults();
            ResetToDefaultsAfterStorage();
        }

        protected void ResetToDefaults(T defaultStorage)
        {
            Storage = defaultStorage;
            ResetToDefaults();
        }

        protected virtual void ResetToDefaultsBeforeStorage() { }
        protected virtual void ResetToDefaultsAfterStorage() { }

        public override void SetValue(string variableName, Value value)
        {
            if (!SetValueNoStorage(variableName, value))
            {
                Storage.SetValue(variableName, value);
            }
        }

        protected virtual bool SetValueNoStorage(string variableName, Value value) => false;
    }
}
