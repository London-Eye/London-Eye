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
        private readonly Dictionary<string, AccessIndex> accessIndices = new Dictionary<string, AccessIndex>();

        protected override Value GetValueAfterStorage(string variableName)
        {
            if (accessIndices.TryGetValue(this.RemoveLeadingIfPresent(variableName), out AccessIndex accessIndex))
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
            var yarnAccessIndices = GetYarnAccesses(objectValue);

            foreach (AccessIndex index in yarnAccessIndices)
            {
                index.Name = GetAccessVariableName(variableName, index.Name);
            }

            AddIndices(yarnAccessIndices);

            if (accessIndices.TryGetValue(this.RemoveLeadingIfPresent(variableName), out AccessIndex accessIndex))
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

        public void AddIndicesFrom(object target)
            => AddIndices(GetYarnAccesses(target));

        public void AddIndices(IEnumerable<AccessIndex> accessIndices)
        {
            foreach (AccessIndex index in accessIndices)
            {
                AddIndex(index);
            }
        }

        public void AddIndex(AccessIndex accessIndex)
        {
            accessIndices[accessIndex.Name] = accessIndex;
        }

        public static List<AccessIndex> GetYarnAccesses(object value)
        {
            List<AccessIndex> res = new List<AccessIndex>();

            Type t = value.GetType();

            YarnRecursiveAccessAttribute[] recursiveAttributes = (YarnRecursiveAccessAttribute[])Attribute.GetCustomAttributes(t, typeof(YarnRecursiveAccessAttribute));
            foreach (YarnRecursiveAccessAttribute attribute in recursiveAttributes)
            {
                res.Add(new AccessIndex(value, attribute));
            }

            foreach (PropertyInfo propertyInfo in t.GetProperties())
            {
                var yarnAccess = GetYarnAccess(value, propertyInfo);
                if (yarnAccess != null) res.Add(yarnAccess);
            }

            foreach (FieldInfo fieldInfo in t.GetFields())
            {
                var yarnAccess = GetYarnAccess(value, fieldInfo);
                if (yarnAccess != null) res.Add(yarnAccess);
            }

            return res;
        }

        /// <summary>
        /// Important: Use only instances of <see cref="PropertyInfo"/> or <see cref="FieldInfo"/> for the <paramref name="memberInfo"/> parameter.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="memberInfo"></param>
        public static AccessIndex GetYarnAccess(object value, MemberInfo memberInfo)
        {
            YarnAccessAttribute attribute = (YarnAccessAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(YarnAccessAttribute));
            if (attribute != null)
            {
                AccessIndex accessIndex;
                if (memberInfo is PropertyInfo propertyInfo)
                {
                    accessIndex = new AccessIndex(value, propertyInfo, attribute.name);
                }
                else if (memberInfo is FieldInfo fieldInfo)
                {
                    accessIndex = new AccessIndex(value, fieldInfo, attribute.name);
                }
                else
                {
                    throw new ArgumentException($"The subtype used for {nameof(memberInfo)} is not valid. Check the documentation for details.");
                }

                return accessIndex;
            }

            return null;
        }

        public static string GetAccessVariableName(string variableName, string accessName)
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
            accessIndices.Clear();
        }
    }
}
