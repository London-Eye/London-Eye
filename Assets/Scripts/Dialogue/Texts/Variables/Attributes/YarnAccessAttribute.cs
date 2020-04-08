using System;

namespace Assets.Scripts.Dialogue.Texts.Variables
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class YarnAccessAttribute : Attribute
    {
        public string name;
    }
}
