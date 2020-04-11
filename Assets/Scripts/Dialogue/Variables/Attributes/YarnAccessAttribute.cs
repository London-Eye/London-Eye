using System;

namespace Assets.Scripts.Dialogue.Variables.Attributes
{
    /// <summary>
    /// Specify to Yarn that this field or property can be accessed via <see cref="Storages.AccessibleVariableStorage{T}"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class YarnAccessAttribute : Attribute
    {
        public string name;
    }
}
