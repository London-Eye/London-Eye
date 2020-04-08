using System;

namespace Assets.Scripts.Dialogue.Texts.Snippets
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    public class YarnRecursiveAccessAttribute : Attribute
    {
        public const string DefaultMemberSeparator = ".";

        public string memberSeparator = DefaultMemberSeparator;

        public string name;

        private readonly string path;

        public YarnRecursiveAccessAttribute(string path)
        {
            this.path = path;
        }

        public object GetValue(object obj)
        {
            string[] memberAccesses = path.Split(memberSeparator.ToCharArray());

            object currentValue = obj;

            foreach (string member in memberAccesses)
            {
                Type currentType = currentValue.GetType();
                currentValue = currentType.GetProperty(member)?.GetValue(currentValue) ?? currentType.GetField(member)?.GetValue(currentValue);
                if (currentValue == null) { throw new InvalidOperationException($"Cannot find access (path: {path})"); }
            }

            return currentValue;
        }
    }

}
