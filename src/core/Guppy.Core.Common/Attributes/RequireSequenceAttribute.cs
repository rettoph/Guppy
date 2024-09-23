namespace Guppy.Core.Common.Attributes
{
    public abstract class RequireSequenceAttribute : Attribute
    {
        public readonly Type SequenceType;

        internal RequireSequenceAttribute(Type sequenceType)
        {
            this.SequenceType = sequenceType;
        }
    }

    public sealed class RequireSequenceAttribute<TSequence> : RequireSequenceAttribute
        where TSequence : unmanaged, Enum
    {
        public RequireSequenceAttribute() : base(typeof(TSequence))
        {

        }
    }
}
