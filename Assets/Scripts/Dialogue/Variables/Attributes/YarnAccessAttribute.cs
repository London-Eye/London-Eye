using System;

namespace Assets.Scripts.Dialogue.Variables.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class YarnAccessAttribute : Attribute
    {
        public string name;
    }
}
