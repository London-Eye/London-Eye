using System;
using Yarn;

namespace Assets.Scripts.Dialogue.Variables.Access
{
    public abstract class AccessIndex
    {
        public string Name { get; set; }

        public object Target { get; }

        public abstract Type AccessType { get; }

        public AccessIndex(string name, object target)
        {
            Name = name;
            Target = target;
        }

        public abstract Value GetValue();
        public abstract void SetValue(object value);
    }
}
