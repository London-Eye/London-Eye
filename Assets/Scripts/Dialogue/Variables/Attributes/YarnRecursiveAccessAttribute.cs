using System;

namespace Assets.Scripts.Dialogue.Variables.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    public class YarnRecursiveAccessAttribute : Attribute
    {
        public const string DefaultMemberSeparator = ".";

        public string memberSeparator = DefaultMemberSeparator;

        public string Name { get; }

        private readonly string path;

        public YarnRecursiveAccessAttribute(string path, string name)
        {
            this.Name = name;
            this.path = path;
        }

        public object GetValue(object obj)
        {
            string[] memberAccesses = path.Split(memberSeparator.ToCharArray());

            object currentValue = obj;

            foreach (string member in memberAccesses)
            {
                currentValue = GoToNextValue(currentValue, member);
            }

            return currentValue;
        }

        public void SetValue(object obj, object value)
        {
            string[] memberAccesses = path.Split(memberSeparator.ToCharArray());

            object currentValue = obj;

            for (int i = 0; i < memberAccesses.Length - 1; i++)
            {
                string member = memberAccesses[i];
                GoToNextValue(currentValue, member);
            }

            Type lastType = currentValue.GetType();
            string lastMember = memberAccesses[memberAccesses.Length - 1];

            var propertyInfo = lastType.GetProperty(lastMember);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(currentValue, value);
            }
            else
            {
                var fieldInfo = lastType.GetField(lastMember);
                if (fieldInfo != null) fieldInfo.SetValue(currentValue, value);
            }
        }

        private object GoToNextValue(object currentValue, string member)
        {
            Type currentType = currentValue.GetType();
            currentValue = currentType.GetProperty(member)?.GetValue(currentValue) ?? currentType.GetField(member)?.GetValue(currentValue);
            if (currentValue == null) { throw new InvalidOperationException($"Cannot find access (path: {path})"); }

            return currentValue;
        }
    }

}
