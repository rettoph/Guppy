namespace Guppy.Core.Common.Attributes
{
    public class SequenceAttribute<TSequenceGroup>(int value) : Attribute
        where TSequenceGroup : unmanaged, Enum
    {
        public readonly int Value = value;
    }
}
