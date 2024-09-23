namespace Guppy.Core.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = true)]
    public class SequenceGroupAttribute<TSequenceGroup> : Attribute
        where TSequenceGroup : Enum
    {
        public readonly TSequenceGroup Value;

        public SequenceGroupAttribute(TSequenceGroup value)
        {
            this.Value = value;
        }
    }
}
