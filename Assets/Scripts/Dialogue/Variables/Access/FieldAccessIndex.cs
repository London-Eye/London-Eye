using Assets.Scripts.Common;
using System.Reflection;

namespace Assets.Scripts.Dialogue.Variables.Access
{
    public class FieldAccessIndex : AccessIndex
    {
        public FieldInfo TargetFieldInfo { get; }
        public override System.Type AccessType => TargetFieldInfo.FieldType;
        public override bool IsStatic => TargetFieldInfo.IsStatic;

        public FieldAccessIndex(object target, FieldInfo fieldInfo, string name = null) : base(name ?? fieldInfo.Name, target)
        {
            TargetFieldInfo = fieldInfo;
        }

        public override Yarn.Value GetValue() => Utilities.AsYarnValue(TargetFieldInfo.GetValue(Target));
        public override void SetValue(object value) => TargetFieldInfo.SetValue(Target, value);
    }
}
