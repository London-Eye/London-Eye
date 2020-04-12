using Assets.Scripts.Common;
using Assets.Scripts.Dialogue.Variables.Attributes;

namespace Assets.Scripts.Dialogue.Variables.Access
{
    public class RecursiveAccessIndex : AccessIndex
    {
        public YarnRecursiveAccessAttribute TargetAttribute { get; }

        private System.Type accessType;
        public override System.Type AccessType => accessType;
        public override bool IsStatic => false;

        public RecursiveAccessIndex(object target, YarnRecursiveAccessAttribute attribute, string name = null)
            : base(name ?? attribute.Name, target)
        {
            TargetAttribute = attribute;
        }

        public override Yarn.Value GetValue()
        {
            object value = TargetAttribute.GetValue(Target);
            if (accessType == null && value != null) accessType = value.GetType();
            return Utilities.AsYarnValue(value);
        }

        public override void SetValue(object value) => TargetAttribute.SetValue(Target, value);
    }
}
