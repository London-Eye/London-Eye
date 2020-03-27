using Assets.Scripts.Dialogue.Texts.Snippets;

namespace Assets.Scripts.Dialogue
{
    public class ComplexDialogueSnippetSystem : DialogueSnippetSystem<object>
    {
        protected override void Awake()
        {
            ComplexSnippetFormat format = new ComplexSnippetFormat(StartSeparator, EndSeparator);

            Format = format;
        }
    }
}
