using System.Collections.Generic;

namespace Assets.Scripts.Dialogue.Texts
{
    public class DialogueText : IDialogueText
    {
        public string Text { get; set; }

        public DialogueText(string text)
        {
            this.Text = text;
        }

        public void AddText(string text)
        {
            this.Text += text;
        }

        public void AddDialogueText(IDialogueText dialogueText)
        {
            this.Text += dialogueText.ToString();
        }

        public IEnumerable<string> Parse()
        {
            string currentText = "";
            foreach (char letter in Text)
            {
                currentText += letter;
                yield return currentText;
            }
        }

        public override string ToString() => this.Text;
    }
}
