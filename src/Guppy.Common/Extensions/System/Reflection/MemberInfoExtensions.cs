using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System.Reflection
{
    public static class MemberInfoExtensions
    {
        public static bool HasCustomAttribute<T>(this MemberInfo member)
            where T : Attribute
        {
            return member.GetCustomAttribute<T>() is not null;
        }

        public static bool HasCustomAttribute<T>(this MemberInfo member, bool inherit)
             where T : Attribute
        {
            return member.GetCustomAttributes<T>(inherit).Count() > 0;
        }
    }
}
