using System.Collections.Generic;

namespace Assets.Scripts.Dialogue.Texts
{
    public interface IDialogueText
    {
        IEnumerable<string> Parse();

        void AddText(string text);
        void AddDialogueText(IDialogueText dialogueText);
    }
}