namespace Guppy.Core.Common.Interfaces
{
    public interface IRuntimeSequence<TSequenceGroup>
        where TSequenceGroup : unmanaged, Enum
    {
        int Value { get; }
    }
}