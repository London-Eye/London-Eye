using Assets.Scripts.Common;
using Assets.Scripts.Dialogue.Variables.Access;
using Assets.Scripts.Dialogue.Variables.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using Yarn;
using Yarn.Unity;

namespace Assets.Scripts.Dialogue.Variables.Storages
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
            string variableNameNoLeading = this.RemoveLeadingIfPresent(variableName);

            var yarnAccessIndices = GetYarnAccesses(objectValue);

            foreach (AccessIndex index in yarnAccessIndices)
            {
                if (!index.IsStatic)
                {
                    index.Name = GetAccessVariableName(variableNameNoLeading, index.Name);
                }
            }

            AddIndices(yarnAccessIndices);

            if (accessIndices.TryGetValue(variableNameNoLeading, out AccessIndex accessIndex))
            {
                if (objectValue is Value yarnValue)
                {
                    Type targetType = accessIndex.AccessType;
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

        public void AddIndexFrom(object target, PropertyInfo propertyInfo)
            => AddIndex(GetYarnAccess(target, propertyInfo));

        public void AddIndexFrom(object target, FieldInfo fieldInfo)
            => AddIndex(GetYarnAccess(target, fieldInfo));

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
                res.Add(new RecursiveAccessIndex(value, attribute));
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

        public static AccessIndex GetYarnAccess(object value, PropertyInfo propertyInfo)
        {
            YarnAccessAttribute attribute = (YarnAccessAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(YarnAccessAttribute));
            return attribute == null ? null : new PropertyAccessIndex(value, propertyInfo, attribute.name);
        }

        public static AccessIndex GetYarnAccess(object value, FieldInfo fieldInfo)
        {
            YarnAccessAttribute attribute = (YarnAccessAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(YarnAccessAttribute));
            return attribute == null ? null : new FieldAccessIndex(value, fieldInfo, attribute.name);
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
