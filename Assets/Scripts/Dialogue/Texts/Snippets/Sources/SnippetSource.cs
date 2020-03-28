using UnityEngine;

namespace Assets.Scripts.Dialogue.Texts.Snippets.Sources
{
    public abstract class SnippetSource<T> : MonoBehaviour
    {
        public abstract bool TryGetValue(string name, out T value);
    }

    public abstract class SnippetSource : SnippetSource<object>
    {
    }
}
