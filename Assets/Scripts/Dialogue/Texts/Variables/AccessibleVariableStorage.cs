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
        private readonly Dictionary<string, Func<Value>> accessDictionary = new Dictionary<string, Func<Value>>();

        protected override Value GetValueAfterStorage(string variableName)
        {
            if (accessDictionary.TryGetValue(variableName, out Func<Value> getValue))
            {
                return getValue();
            }
            else
            {
                return Value.NULL;
            }
        }

        public void SetValue(string variableName, object value)
        {
            Type t = value.GetType();

            YarnRecursiveAccessAttribute[] recursiveAttributes = (YarnRecursiveAccessAttribute[])Attribute.GetCustomAttributes(t, typeof(YarnRecursiveAccessAttribute));
            foreach (YarnRecursiveAccessAttribute attribute in recursiveAttributes)
            {
                string accessVariableName = GetAccessVariableName(variableName, (attribute.name ?? t.Name));
                accessDictionary[accessVariableName] = () => Utilities.AsYarnValue(attribute.GetValue(value));
            }

            foreach (PropertyInfo propertyInfo in t.GetProperties())
            {
                CheckYarnAccess(variableName, value, propertyInfo);
            }

            foreach (FieldInfo fieldInfo in t.GetFields())
            {
                CheckYarnAccess(variableName, value, fieldInfo);
            }

            Value yarnValue = null;
            try
            {
                yarnValue = Utilities.AsYarnValue(value);
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

                Func<Value> accessFunc;
                if (memberInfo is PropertyInfo propertyInfo)
                {
                    accessFunc = () => Utilities.AsYarnValue(propertyInfo.GetValue(value));
                }
                else if (memberInfo is FieldInfo fieldInfo)
                {
                    accessFunc = () => Utilities.AsYarnValue(fieldInfo.GetValue(value));
                }
                else
                {
                    throw new ArgumentException($"The subtype used for {nameof(memberInfo)} is not valid. Check the documentation for details.");
                }

                accessDictionary[accessVariableName] = accessFunc;
            }
        }

        private static string GetAccessVariableName(string variableName, string accessName)
            => variableName + accessName;

        /// <summary>
        /// Don't use this method. It won't use the object access functionality. For that, use <see cref="SetValue(string, object)"/> instead.
        /// <para>If you don't need the object access functionality, then you don't need to use this class at all.</para>
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="value"></param>
        public override void SetValue(string variableName, Value value)
        {
            throw new InvalidOperationException($"This operation is not supported. Check this method's documentation for details.");
        }

        protected override void ResetToDefaultsAfterStorage()
        {
            accessDictionary.Clear();
        }
    }
}
