namespace Assets.Scripts.Dialogue.Texts.Snippets
{
    public class ComplexSnippetFormat : SnippetFormat<object>
    {
        private const string DefaultStartAccessSeparator = "[", DefaultEndAccessSeparator = "]",
            DefaultAccessMemberSeparator = ".";

        public string StartAccessSeparator { get; set; }
        public string EndAccessSeparator { get; set; }
        public string AccessMemberSeparator { get; set; }

        public ComplexSnippetFormat(string startSeparator, string endSeparator, string startAccessSeparator = DefaultStartAccessSeparator, string endAccessSeparator = DefaultEndAccessSeparator, string accessMemberSeparator = DefaultAccessMemberSeparator) : base(startSeparator, endSeparator)
        {
            StartAccessSeparator = startAccessSeparator;
            EndAccessSeparator = endAccessSeparator;
            AccessMemberSeparator = accessMemberSeparator;
        }

        public override Snippet<object> CreateSnippet(string name)
        {
            int indexOfStartAccess = name.IndexOf(StartAccessSeparator);
            if (indexOfStartAccess >= 0 && Snippets.TryGetValue(name.Substring(0, indexOfStartAccess), out object value))
            {
                int indexOfEndAccess = name.IndexOf(EndAccessSeparator);
                if (indexOfEndAccess >= 0)
                {
                    string access = name.Substring(indexOfStartAccess + 1, indexOfEndAccess - 1 - indexOfStartAccess);
                    string[] memberAccesses = access.Split(AccessMemberSeparator.ToCharArray());

                    object currentValue = value;

                    foreach (string member in memberAccesses)
                    {
                        System.Type currentType = currentValue.GetType();
                        currentValue = currentType.GetProperty(member)?.GetValue(currentValue) ?? currentType.GetField(member)?.GetValue(currentValue);
                        if (currentValue == null) { throw new ParsingException(indexOfStartAccess, "Cannot find access"); }
                    }

                    return new Snippet<object>(name, currentValue, this);
                }
                else
                {
                    throw new ParsingException(indexOfStartAccess, "Start access without end access");
                }
            }
            return base.CreateSnippet(name);
        }
    }
}
