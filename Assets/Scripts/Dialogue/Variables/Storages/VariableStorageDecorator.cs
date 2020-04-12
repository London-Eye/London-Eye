using Yarn;
using Yarn.Unity;

namespace Assets.Scripts.Dialogue.Variables.Storages
{
    public abstract class VariableStorageDecorator<T> : VariableStorageBehaviour where T : VariableStorageBehaviour
    {
        public bool allowReset, persistStorageOnReset;

        public T Storage;

        /// Reset to our default values when the game starts
        internal void Awake()
        {
            T initialStorage = InitStorage();
            if (initialStorage != null) Storage = initialStorage;
        }

        protected virtual T InitStorage() => null;

        public override Value GetValue(string variableName)
        {
            Value value = GetValueBeforeStorage(variableName);
            if (value == Value.NULL)
            {
                if (Storage != null) value = Storage.GetValue(variableName);
                if (Storage == null || value == Value.NULL)
                {
                    value = GetValueAfterStorage(variableName);
                }
            }
            return value;
        }

        protected virtual Value GetValueBeforeStorage(string variableName) => Value.NULL;
        protected virtual Value GetValueAfterStorage(string variableName) => Value.NULL;

        public override void ResetToDefaults() => ResetToDefaults(allowReset);

        public void ResetToDefaultsForce() => ResetToDefaults(true);

        private void ResetToDefaults(bool allowReset)
        {
            if (allowReset)
            {
                ResetToDefaultsBeforeStorage();

                if (!persistStorageOnReset && Storage != null)
                {
                    Storage.ResetToDefaults();
                }

                ResetToDefaultsAfterStorage();
            }
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
                if (Storage != null) Storage.SetValue(variableName, value);
                else throw new System.InvalidOperationException("Unable to set value");
            }
        }

        protected virtual bool SetValueNoStorage(string variableName, Value value) => false;
    }
}
