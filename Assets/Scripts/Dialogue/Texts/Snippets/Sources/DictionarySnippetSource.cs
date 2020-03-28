using System.Collections.Generic;

namespace Assets.Scripts.Dialogue.Texts.Snippets.Sources
{
    public abstract class DictionarySnippetSource<T> : SnippetSource<T>
    {
        public Dictionary<string, T> Snippets { get; set; } = new Dictionary<string, T>();

        public override bool TryGetValue(string name, out T value)
            => Snippets.TryGetValue(name, out value);
    }

    public class DictionarySnippetSource : SnippetSource
    {
        public Dictionary<string, object> Snippets { get; set; } = new Dictionary<string, object>();

        public override bool TryGetValue(string name, out object value)
            => Snippets.TryGetValue(name, out value);
    }
}
