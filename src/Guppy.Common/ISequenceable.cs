namespace Guppy.Common
{
    public interface ISequenceable<TSequence>
        where TSequence : unmanaged, Enum
    {
        public TSequence? Sequence => null;
    }
}
