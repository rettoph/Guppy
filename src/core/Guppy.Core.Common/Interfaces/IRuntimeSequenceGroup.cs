namespace Guppy.Core.Common.Interfaces
{
    public interface IRuntimeSequenceGroup<TSequenceGroup>
        where TSequenceGroup : unmanaged, Enum
    {
        SequenceGroup<TSequenceGroup> Value { get; }
    }
}