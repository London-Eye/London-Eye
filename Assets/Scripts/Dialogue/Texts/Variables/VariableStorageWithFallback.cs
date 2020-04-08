using Yarn;
using Yarn.Unity;

namespace Assets.Scripts.Dialogue.Texts.Variables
{
    public abstract class VariableStorageWithFallback<T> : VariableStorageBehaviour where T : VariableStorageBehaviour
    {
        public T Fallback { get; private set; }

        private void Start()
        {
            Fallback = gameObject.AddComponent<T>();
        }

        public override Value GetValue(string variableName)
        {
            Value value = GetValueBeforeFallback(variableName);
            if (value == Value.NULL)
            {
                value = Fallback.GetValue(variableName);
                if (value == Value.NULL)
                {
                    value = GetValueAfterFallback(variableName);
                }
            }
            return value;
        }

        protected virtual Value GetValueBeforeFallback(string variableName) => Value.NULL;
        protected virtual Value GetValueAfterFallback(string variableName) => Value.NULL;

        public override void ResetToDefaults()
        {
            ResetToDefaultsBeforeFallback();
            Fallback.ResetToDefaults();
            ResetToDefaultsAfterFallback();
        }

        protected virtual void ResetToDefaultsBeforeFallback() { }
        protected virtual void ResetToDefaultsAfterFallback() { }

        public override void SetValue(string variableName, Value value)
        {
            if (!SetValueNoFallback(variableName, value))
            {
                Fallback.SetValue(variableName, value);
            }
        }

        protected abstract bool SetValueNoFallback(string variableName, Value value);
    }
}
