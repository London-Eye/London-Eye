using Assets.Scripts.Common;
using Assets.Scripts.Dialogue.Texts.Variables.Attributes;
using System;
using System.Reflection;
using Yarn;

namespace Assets.Scripts.Dialogue.Texts.Variables
{
    public class AccessIndex
    {
        public string Name { get; set; }

        public object Target { get; private set; }

        public MemberInfo TargetMemberInfo { get; private set; }

        public Type MemberType
        {
            get
            {
                if (TargetMemberInfo is PropertyInfo propertyInfo)
                {
                    return propertyInfo.PropertyType;
                }
                else if (TargetMemberInfo is FieldInfo fieldInfo)
                {
                    return fieldInfo.FieldType;
                }
                else
                {
                    return null;
                }
            }
        }

        private readonly Func<Value> getValue;
        private readonly Action<object> setValue;

        private AccessIndex(string name, object target, MemberInfo memberInfo)
        {
            Name = name ?? memberInfo.Name;
            Target = target;
            TargetMemberInfo = memberInfo;
        }

        public AccessIndex(object target, PropertyInfo propertyInfo, string name = null) : this(name, target, memberInfo: propertyInfo)
        {
            getValue = () => Utilities.AsYarnValue(propertyInfo.GetValue(target));
            setValue = (value) => propertyInfo.SetValue(target, value);
        }

        public AccessIndex(object target, FieldInfo fieldInfo, string name = null) : this(name, target, memberInfo: fieldInfo)
        {
            getValue = () => Utilities.AsYarnValue(fieldInfo.GetValue(target));
            setValue = (value) => fieldInfo.SetValue(target, value);
        }

        public AccessIndex(object target, YarnRecursiveAccessAttribute attribute, string name = null) : this(name ?? attribute.name, target, memberInfo: null)
        {
            getValue = () => Utilities.AsYarnValue(attribute.GetValue(target));
            setValue = (value) => attribute.SetValue(target, value);
        }

        public Value GetValue() => getValue();
        public void SetValue(object value) => setValue(value);
    }
}
