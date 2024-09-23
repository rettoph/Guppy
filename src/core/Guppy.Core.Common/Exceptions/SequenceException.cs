using Guppy.Core.Common.Extensions.System;
using System.Reflection;

namespace Guppy.Core.Common.Exceptions
{
    public class SequenceException : Exception
    {
        public readonly Type Sequence;
        public readonly MemberInfo? Member;

        public SequenceException(Type sequence) : base($"Attepted sequence on null")
        {
            this.Sequence = sequence;
        }

        public SequenceException(Type sequence, MemberInfo member) : base($"Missing sequence '{sequence.ToString()}' on member {GetMemberString(member)}")
        {
            this.Sequence = sequence;
            this.Member = member;
        }

        private static string GetMemberString(MemberInfo member)
        {
            if (member is MethodInfo methodInfo)
            {
                return $"{methodInfo.DeclaringType?.GetFormattedName()}::{methodInfo.Name}";
            }

            return member.ToString() ?? member.Name;
        }
    }
}
