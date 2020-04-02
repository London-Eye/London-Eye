using System;
using System.Text;

namespace Assets.Scripts.Dialogue.Texts
{
    [Serializable]
    public class ParsingException : FormatException
    {
        public const string END_ACTION_MESSAGE = "Skipping source.";

        public int? Index { get; }
        public int? LineNumber { get; }

        public ParsingException(string message = null, int? index = null, int? lineNumber = null, string endActionMessage = END_ACTION_MESSAGE) : base(GetFullMessage(message, lineNumber, index, endActionMessage))
        {
            this.Index = index;
            this.LineNumber = lineNumber;
        }

        protected static string GetFullMessage(string message, int? lineNumber, int? index, string endMessage = null)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(message);
            if (lineNumber.HasValue || index.HasValue)
            {
                builder.Append(" (");
                if (lineNumber.HasValue)
                {
                    builder.Append($"line {lineNumber}");
                    if (index.HasValue)
                    {
                        builder.Append(", ");
                    }
                }
                if (index.HasValue)
                {
                    builder.Append($"position {index}");
                }
                builder.Append(").");
            }
            if (endMessage != null)
            {
                builder.Append($" {endMessage}");
            }
            return builder.ToString();
        }

        [Serializable]
        public class StartTagSeparatorWithoutEndException : ParsingException
        {
            public const string DEFAULT_MESSAGE = "Start tag separator without end";
            public StartTagSeparatorWithoutEndException(int? index = null, int? lineNumber = null) : base(DEFAULT_MESSAGE, index, lineNumber)
            {
            }
        }
    }
}
