using Assets.Scripts.Dialogue.Texts.Snippets.Sources;
using System.Collections.Generic;

namespace Assets.Scripts.Dialogue.Texts.Snippets
{
    public class SnippetFormat<T> : ISeparatedFormat<Snippet<T>>
    {
        public string StartSeparator { get; }
        public string EndSeparator { get; }

        public SnippetSource<T> Source { get; set; }

        public SnippetFormat(string startSeparator, string endSeparator, SnippetSource<T> source)
        {
            StartSeparator = startSeparator;
            EndSeparator = endSeparator;

            Source = source;
        }

        public int IndexOfNextStart(string text) => text.IndexOf(StartSeparator);
        public int IndexOfNextEnd(string text) => text.IndexOf(EndSeparator);

        public bool HasAnyTags(string text) => IndexOfNextStart(text) >= 0;

        public string GetFullName(string name) => $"{StartSeparator}{name}{EndSeparator}".Trim();

        public virtual Snippet<T> CreateSnippet(string name)
        {
            if (Source.TryGetValue(name, out T value))
            {
                return new Snippet<T>(name, value, this);
            }
            else
            {
                throw NameWithoutValueException(name);
            }
        }

        public Snippet<T> Extract(string line, out int startingIndex, out int endIndex, out string remainingText)
        {
            Snippet<T> snippet = null;

            startingIndex = IndexOfNextStart(line);
            if (startingIndex >= 0)
            {
                int nextIndex = startingIndex + 1;
                string textSearchingForEnd = line.Substring(nextIndex);
                int indexOfSnippetEnd = IndexOfNextEnd(textSearchingForEnd);

                if (indexOfSnippetEnd >= 0)
                {
                    nextIndex = line.Length - textSearchingForEnd.Length + indexOfSnippetEnd + 1;
                    string snippetName = line.Substring(startingIndex + StartSeparator.Length, nextIndex - (EndSeparator.Length + 1) - startingIndex);

                    endIndex = indexOfSnippetEnd + EndSeparator.Length;
                    remainingText = textSearchingForEnd.Substring(endIndex);
                    snippet = CreateSnippet(snippetName);
                }
                else
                {
                    throw new ParsingException(line.Length - textSearchingForEnd.Length + startingIndex, "Snippet init without end");
                }
            }
            else
            {
                remainingText = line;
                endIndex = -1;
            }

            return snippet;
        }

        protected static KeyNotFoundException NameWithoutValueException(string name)
            => new KeyNotFoundException($"Snippet of name \"{name}\" does not have an associated value.");
    }
}
