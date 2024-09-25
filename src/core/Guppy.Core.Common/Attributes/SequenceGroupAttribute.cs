namespace Guppy.Core.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method | AttributeTargets.Field, AllowMultiple = true)]
    public class SequenceGroupAttribute<T> : Attribute
        where T : unmanaged, Enum
    {
        public readonly SequenceGroup<T> Value;

        public SequenceGroupAttribute(string name, int sequence = 0)
        {
            this.Value = new SequenceGroup<T>(name, sequence);
        }

        public SequenceGroupAttribute(T value)
        {
            this.Value = SequenceGroup<T>.GetByValue(value);
        }
    }
}
