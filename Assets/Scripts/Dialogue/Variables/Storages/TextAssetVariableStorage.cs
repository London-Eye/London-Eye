using System.IO;
using UnityEngine;
using Yarn;
using Yarn.Unity;

namespace Assets.Scripts.Dialogue.Variables.Storages
{

    public class TextAssetVariableStorage : VariableStorageDecorator<InMemoryVariableStorage>
    {
        public TextAsset textAsset;

        public const string DefaultNameValueSeparator = "===";

        public string NameValueSeparator = DefaultNameValueSeparator;

        private void Start()
        {
            ResetToDefaults();
        }

        public int IndexOfNextNameValueSeparator(string text) => text.IndexOf(NameValueSeparator);

        protected override bool SetValueNoStorage(string variableName, Value value)
        {
            throw new System.InvalidOperationException(VariableStorageGroup.ReadOnlyVariableStorageMessage);
        }

        protected override void ResetToDefaultsAfterStorage()
        {
            using (StringReader textReader = new StringReader(textAsset.text))
            {
                string line;
                while ((line = textReader.ReadLine()) != null)
                {
                    int splitIndex = IndexOfNextNameValueSeparator(line);
                    string name = line.Substring(0, splitIndex), value = line.Substring(splitIndex + NameValueSeparator.Length);
                    Storage.SetValue(name, value);
                }
            }
        }
    }
}
