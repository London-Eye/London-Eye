using System;

namespace Assets.Scripts.Dialogue.Texts.Snippets
{
    [Serializable]
    public class Snippet<T>
    {
        public string Name { get; set; }

        public T Value { get; private set; }

        public SnippetFormat<T> Format { get; set; }

        public string FullName => Format.GetFullName(Name);

        public Snippet(string name, T value, SnippetFormat<T> format)
        {
            Name = name;
            Value = value;
            Format = format;
        }
    }
}
