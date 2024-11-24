namespace Guppy.Core.Common.Attributes
{
    /// <summary>
    /// Determins the members sequence group
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field, AllowMultiple = false)]
    public class SequenceGroupAttribute<T> : Attribute
        where T : unmanaged, Enum
    {
        public readonly SequenceGroup<T> Value;

        public SequenceGroupAttribute(string name, int order = 0)
        {
            this.Value = new SequenceGroup<T>(name, order);
        }

        public SequenceGroupAttribute(T value)
        {
            this.Value = SequenceGroup<T>.GetByValue(value);
        }
    }
}
