using Yarn;

namespace Assets.Scripts.Dialogue.Texts.Snippets.Sources
{
    public class VariableSnippetSource : SnippetSource
    {
        public VariableStorage VariableStorage;

        public override bool TryGetValue(string name, out object value)
        {
            value = VariableStorage.GetValue(name);
            return value != null;
        }
    }
}
