using System.Reflection;
using Guppy.Core.Common.Extensions.System.Reflection;

namespace Guppy.Core.Common.Extensions.System
{
    public static class EnumExtensions
    {
        public static MemberInfo GetMemberInfo(this Enum value)
        {
            MemberInfo[] memberInfo = value.GetType().GetMember(value.ToString());
            if (memberInfo.Length == 0)
            {
                throw new InvalidOperationException();
            }

            if (memberInfo.Length > 1)
            {
                throw new InvalidOperationException();
            }

            return memberInfo[0];
        }

        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>(this Enum value)
            where TAttribute : Attribute => value.GetMemberInfo().GetAllCustomAttributes<TAttribute>(false);
    }
}