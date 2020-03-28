using System.IO;

namespace Assets.Scripts.Dialogue.Texts.Snippets.Sources
{
    public class SimpleTextFileSnippetSource : TextFileSnippetSource
    {
        public const string DefaultNameValueSeparator = "===";

        public string NameValueSeparator = DefaultNameValueSeparator;

        public int IndexOfNextNameValueSeparator(string text) => text.IndexOf(NameValueSeparator);

        protected override void LoadSnippets(string text)
        {
            using (StringReader textReader = new StringReader(text))
            {
                string line;
                while ((line = textReader.ReadLine()) != null)
                {
                    int splitIndex = IndexOfNextNameValueSeparator(line);
                    string name = line.Substring(0, splitIndex), value = line.Substring(splitIndex + NameValueSeparator.Length);
                    Snippets[name] = value;
                }
            }
        }
    }
}
