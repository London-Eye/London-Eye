using Assets.Scripts.Common;
using Assets.Scripts.Dialogue.Texts.Variables.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using Yarn;
using Yarn.Unity;

namespace Assets.Scripts.Dialogue.Texts.Variables
{
    public class AccessibleVariableStorage<T> : VariableStorageDecorator<T> where T : VariableStorageBehaviour
    {
        private struct AccessIndex
        {
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

            public AccessIndex(object target, PropertyInfo propertyInfo)
            {
                Target = target;
                TargetMemberInfo = propertyInfo;

                getValue = () => Utilities.AsYarnValue(propertyInfo.GetValue(target));
                setValue = (value) => propertyInfo.SetValue(target, value);
            }

            public AccessIndex(object target, FieldInfo fieldInfo)
            {
                Target = target;
                TargetMemberInfo = fieldInfo;

                getValue = () => Utilities.AsYarnValue(fieldInfo.GetValue(target));
                setValue = (value) => fieldInfo.SetValue(target, value);
            }

            public AccessIndex(object target, YarnRecursiveAccessAttribute attribute)
            {
                Target = target;
                TargetMemberInfo = null;

                getValue = () => Utilities.AsYarnValue(attribute.GetValue(target));
                setValue = (value) => attribute.SetValue(target, value);
            }

            public Value GetValue() => getValue();
            public void SetValue(object value) => setValue(value);
        }

        private readonly Dictionary<string, AccessIndex> accessIndexes = new Dictionary<string, AccessIndex>();

        protected override Value GetValueAfterStorage(string variableName)
        {
            if (accessIndexes.TryGetValue(variableName, out AccessIndex accessIndex))
            {
                return accessIndex.GetValue();
            }
            else
            {
                return Value.NULL;
            }
        }

        public void SetValue(string variableName, object objectValue)
        {
            CheckYarnAccesses(variableName, objectValue);

            if (accessIndexes.TryGetValue(variableName, out AccessIndex accessIndex))
            {
                if (objectValue is Value yarnValue)
                {
                    Type targetType = accessIndex.MemberType;
                    objectValue = yarnValue.As(targetType);
                }
                accessIndex.SetValue(objectValue);
            }
            else
            {
                Value yarnValue = null;
                try
                {
                    yarnValue = Utilities.AsYarnValue(objectValue);
                }
                catch (ArgumentException)
                {
                    // If the yarn Value couldn't be created, then the type is not supported by Yarn.
                    // In that case, just omit it.
                }

                if (yarnValue != null)
                {
                    base.SetValue(variableName, yarnValue);
                }
            }
        }

        private void CheckYarnAccesses(string variableName, object value)
        {
            Type t = value.GetType();

            YarnRecursiveAccessAttribute[] recursiveAttributes = (YarnRecursiveAccessAttribute[])Attribute.GetCustomAttributes(t, typeof(YarnRecursiveAccessAttribute));
            foreach (YarnRecursiveAccessAttribute attribute in recursiveAttributes)
            {
                string accessVariableName = GetAccessVariableName(variableName, (attribute.name ?? t.Name));
                accessIndexes[accessVariableName] = new AccessIndex(value, attribute);
            }

            foreach (PropertyInfo propertyInfo in t.GetProperties())
            {
                CheckYarnAccess(variableName, value, propertyInfo);
            }

            foreach (FieldInfo fieldInfo in t.GetFields())
            {
                CheckYarnAccess(variableName, value, fieldInfo);
            }
        }

        /// <summary>
        /// Important: Use only instances of <see cref="PropertyInfo"/> or <see cref="FieldInfo"/> for the <paramref name="memberInfo"/> parameter.
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="value"></param>
        /// <param name="memberInfo"></param>
        private void CheckYarnAccess(string variableName, object value, MemberInfo memberInfo)
        {
            YarnAccessAttribute attribute = (YarnAccessAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(YarnAccessAttribute));
            if (attribute != null)
            {
                string accessVariableName = GetAccessVariableName(variableName, (attribute.name ?? memberInfo.Name));

                AccessIndex accessIndex;
                if (memberInfo is PropertyInfo propertyInfo)
                {
                    accessIndex = new AccessIndex(value, propertyInfo);
                }
                else if (memberInfo is FieldInfo fieldInfo)
                {
                    accessIndex = new AccessIndex(value, fieldInfo);
                }
                else
                {
                    throw new ArgumentException($"The subtype used for {nameof(memberInfo)} is not valid. Check the documentation for details.");
                }

                accessIndexes[accessVariableName] = accessIndex;
            }
        }

        private static string GetAccessVariableName(string variableName, string accessName)
            => variableName + accessName;

        /// <summary>
        /// This method is used by Yarn to set values. If you want to set values by code, you should probably use <see cref="SetValue(string, object)"/> instead.
        /// <para>(This method only accepts Yarn <see cref="Value"/>s, so if you wrap your object in a Yarn <see cref="Value"/>, object access will only work for the Yarn <see cref="Value"/>, not your object)</para>
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="value"></param>
        public override void SetValue(string variableName, Value value) => SetValue(variableName, objectValue: value);

        protected override void ResetToDefaultsAfterStorage()
        {
            accessIndexes.Clear();
        }
    }
}
