using Assets.Scripts.Dialogue.Texts.Tags;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Dialogue.Texts
{
    public class DialogueTaggedText : IDialogueText
    {
        public IDialogueText Text { get; set; }
        public Tag Tag { get; set; }

        public string FullText => Tag?.GetTaggedText(Text.ToString());

        public DialogueTaggedText(Tag tag, string text) : this(tag, new DialogueText(text))
        {
        }

        public DialogueTaggedText(Tag tag, IDialogueText dialogueText = null)
        {
            this.Tag = tag;
            this.Text = dialogueText;
        }

        public void AddText(string text)
        {
            if (this.Text == null)
                this.Text = new DialogueText(text);
            else
                this.Text.AddText(text);
        }

        public void AddDialogueText(IDialogueText dialogueText)
        {
            if (this.Text == null)
                this.Text = dialogueText;
            else
                this.Text.AddDialogueText(dialogueText);
        }

        public IEnumerable<string> Parse() => Tag.Parse(Text.Parse);

        public override string ToString() => this.FullText;


        /// <summary>
        /// Analiza el <paramref name="text"/> indicado, y lo clasifica según si contiene o no tags.
        /// <para>Opcionalmente, puedes añadir un <paramref name="logger"/> para que te reporte las excepciones internas que se puedan producir (mediante <see cref="ParsingException"/>).</para>
        /// </summary>
        /// <param name="text">El texto de entrada.</param>
        /// <param name="logger">Función que te reporte las excepciones internas que se puedan producir.</param>
        /// <param name="format">El formato de Tag que deseas que analice dentro del texto.</param>
        /// <returns></returns>
        public static IDialogueText AnalyzeText(string text, TagFormat format, Action<ParsingException> logger = null)
        {
            IDialogueText resultDialogueText = null;

            string textBeingAnalyzed = text;

            while (textBeingAnalyzed != null && textBeingAnalyzed.Length > 0)
            {
                int indexOfTagInit = 0;
                try
                {
                    TagOption tag = format.Extract(textBeingAnalyzed, out indexOfTagInit, out int _, out string remainingTextAfterStart);
                    if (tag != null)
                    {
                        if (indexOfTagInit > 0)
                        {
                            string textBeforeTag = textBeingAnalyzed.Substring(0, indexOfTagInit);
                            if (resultDialogueText == null)
                            {
                                resultDialogueText = new ComplexDialogueText(textBeforeTag);
                            }
                            else
                            {
                                resultDialogueText.AddText(textBeforeTag);
                            }
                        }

                        if (tag.Position == TagOptionPosition.start)
                        {
                            string textSearchingForEnd = remainingTextAfterStart;
                            string taggedText = null, remainingTextAfterEnd = null;
                            TagOption endTag = null;
                            while (taggedText == null && textSearchingForEnd != null && textSearchingForEnd.Length > 0)
                            {
                                endTag = format.Extract(textSearchingForEnd, out int indexOfEndTagInit, out int _, out remainingTextAfterEnd);
                                if (endTag != null)
                                {
                                    if (TagOption.Matches(tag, endTag))
                                    {
                                        taggedText = remainingTextAfterStart.Substring(0, remainingTextAfterStart.Length - remainingTextAfterEnd.Length - endTag.Text.Length);
                                        textBeingAnalyzed = remainingTextAfterEnd; // This tag has been found correctly, go to the next portion of the text
                                        break;
                                    }
                                }
                                textSearchingForEnd = remainingTextAfterEnd;
                            }

                            if (taggedText == null)
                            {
                                throw new TagException.StartTagWithoutEndException(tag, index: text.Length - textBeingAnalyzed.Length + indexOfTagInit);
                            }
                            else
                            {
                                DialogueTaggedText dialogueTaggedText = new DialogueTaggedText(new Tag(tag, endTag), AnalyzeText(taggedText, format, logger));
                                if (resultDialogueText == null)
                                {
                                    if (remainingTextAfterEnd != null && remainingTextAfterEnd.Length > 0)
                                    {
                                        resultDialogueText = new ComplexDialogueText(dialogueTaggedText);
                                    }
                                    else
                                    {
                                        resultDialogueText = dialogueTaggedText;
                                    }
                                }
                                else
                                {
                                    resultDialogueText.AddDialogueText(dialogueTaggedText);
                                }
                            }
                        }
                        else
                        {
                            throw new TagException.EndTagBeforeStartException(tag, index: text.Length - textBeingAnalyzed.Length + indexOfTagInit);
                        }
                    }
                    else
                    {
                        if (resultDialogueText == null)
                        {
                            return new DialogueText(textBeingAnalyzed);
                        }
                        else
                        {
                            resultDialogueText.AddText(textBeingAnalyzed);
                            return resultDialogueText;
                        }
                    }
                }
                catch (ParsingException ex)
                {
                    // Log the exception
                    logger?.Invoke(ex);

                    int nextIndex = indexOfTagInit + 1;

                    // Add the start of the tag as raw text

                    if (resultDialogueText == null)
                    {
                        resultDialogueText = new ComplexDialogueText(textBeingAnalyzed.Substring(0, nextIndex));
                    }
                    else
                    {
                        resultDialogueText.AddText(textBeingAnalyzed[indexOfTagInit].ToString());
                    }

                    // Go to the next portion of the text (Skip the exception source)
                    textBeingAnalyzed = nextIndex < textBeingAnalyzed.Length ? textBeingAnalyzed.Substring(nextIndex) : null;
                }
            }

            return resultDialogueText;
        }
    }
}
