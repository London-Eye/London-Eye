using System;

namespace Assets.Scripts.Dialogue.Texts.Variables.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class YarnAccessAttribute : Attribute
    {
        public string name;
    }
}
