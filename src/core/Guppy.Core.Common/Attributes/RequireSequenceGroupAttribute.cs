namespace Guppy.Core.Common.Attributes
{
    public abstract class RequireSequenceGroupAttribute : Attribute
    {
        public readonly Type SequenceGroupType;

        internal RequireSequenceGroupAttribute(Type sequenceGroupType)
        {
            this.SequenceGroupType = sequenceGroupType;
        }
    }

    public sealed class RequireSequenceGroupAttribute<TSequence> : RequireSequenceGroupAttribute
        where TSequence : unmanaged, Enum
    {
        public RequireSequenceGroupAttribute() : base(typeof(TSequence))
        {

        }
    }
}
