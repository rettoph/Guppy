namespace Guppy.Core.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = true)]
    public class SequenceAttribute<TSequence> : Attribute
        where TSequence : Enum
    {
        public readonly TSequence Value;

        public SequenceAttribute(TSequence value)
        {
            this.Value = value;
        }
    }
}
