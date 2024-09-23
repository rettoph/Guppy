using System.Reflection;

namespace Guppy.Core.Common.Exceptions
{
    public class SequenceException : Exception
    {
        public readonly MethodInfo? MethodInfo;
        public readonly object? Instance;
        public readonly Type Sequence;

        public SequenceException(Type sequence) : base($"Attepted sequence on null")
        {
            this.Sequence = sequence;
        }

        public SequenceException(Type sequence, object instance, MethodInfo methodInfo) : base($"Missing sequence '{sequence.Name}' on type {instance.GetType().GetFormattedName()}::{methodInfo.Name}")
        {
            this.Sequence = sequence;
            this.Instance = instance;
            this.MethodInfo = methodInfo;
        }

        public SequenceException(Type sequence, object instance) : base($"Missing sequence '{sequence.Name}' on type {instance.GetType().GetFormattedName()}")
        {
            this.Sequence = sequence;
            this.Instance = instance;
        }
    }
}
