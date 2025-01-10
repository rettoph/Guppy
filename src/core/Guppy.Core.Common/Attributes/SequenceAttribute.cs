namespace Guppy.Core.Common.Attributes
{
    /// <summary>
    /// Give the member a custom sequence value. This defines the position
    /// of the memeber within its defined sequence group when sequenced
    /// </summary>
    /// <typeparam name="TSequenceGroup"></typeparam>
    /// <param name="value"></param>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SequenceAttribute<TSequenceGroup>(int value) : Attribute
        where TSequenceGroup : unmanaged, Enum
    {
        public readonly int Value = value;
    }
}