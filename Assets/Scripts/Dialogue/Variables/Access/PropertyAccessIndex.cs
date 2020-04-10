using Assets.Scripts.Common;
using System.Reflection;

namespace Assets.Scripts.Dialogue.Variables.Access
{
    public class PropertyAccessIndex : AccessIndex
    {
        public PropertyInfo TargetPropertyInfo { get; }
        public override System.Type AccessType => TargetPropertyInfo.PropertyType;

        public PropertyAccessIndex(object target, PropertyInfo propertyInfo, string name = null)
            : base(name ?? propertyInfo.Name, target)
        {
            TargetPropertyInfo = propertyInfo;
        }

        public override Yarn.Value GetValue() => Utilities.AsYarnValue(TargetPropertyInfo.GetValue(Target));
        public override void SetValue(object value) => TargetPropertyInfo.SetValue(Target, value);
    }
}
