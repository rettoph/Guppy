using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Exceptions;
using System.Reflection;

namespace Guppy.Core.Common.Extensions.System.Reflection
{
    public static class MemberInfoExtensions
    {
        public static bool HasCustomAttribute<T>(this MemberInfo member)
            where T : Attribute
        {
            return member.GetCustomAttributes<T>().Any();
        }

        public static bool HasCustomAttribute<T>(this MemberInfo member, bool inherit)
             where T : Attribute
        {
            return member.GetCustomAttributes(inherit).Any(x => x is T);
        }

        /// <summary>
        /// Attempt to get all an array of all custom attributes mapped to a member.
        /// If <paramref name="inherit"/> is true, then all mapped interfaces will be
        /// checked as well.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="memberInfo"></param>
        /// <param name="inherit"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public static IEnumerable<TAttribute> GetAllCustomAttributes<TAttribute>(this MemberInfo memberInfo, bool inherit)
            where TAttribute : Attribute
        {
            List<TAttribute> result = new List<TAttribute>();

            foreach (TAttribute attribute in memberInfo.GetCustomAttributes(inherit).OfType<TAttribute>())
            {
                yield return attribute;
            }

            if (inherit == false)
            { // No interface checking needed
                yield break;
            }

            if (memberInfo is Type type)
            {
                foreach (Type implementedInterfaceType in type.GetInterfaces())
                {
                    foreach (TAttribute attribute in implementedInterfaceType.GetCustomAttributes(inherit).OfType<TAttribute>())
                    {
                        yield return attribute;
                    }
                }
            }

            if (memberInfo is MethodInfo methodInfo)
            {
                Type declaringType = methodInfo.DeclaringType ?? throw new NotImplementedException();
                foreach (Type implementedInterfaceType in declaringType.GetInterfaces())
                {
                    InterfaceMapping implementedInterfaceMapping = methodInfo.DeclaringType.GetInterfaceMap(implementedInterfaceType);
                    int methodInfoMapIndex = Array.IndexOf(implementedInterfaceMapping.TargetMethods, methodInfo);

                    if (methodInfoMapIndex == -1)
                    {
                        continue;
                    }

                    MethodInfo mappedInterfaceMethodInfo = implementedInterfaceMapping.InterfaceMethods[methodInfoMapIndex];
                    foreach (TAttribute attribute in mappedInterfaceMethodInfo.GetCustomAttributes(inherit).OfType<TAttribute>())
                    {
                        yield return attribute;
                    }
                }
            }
        }

        public static bool TryGetAllCustomAttributes<TAttribute>(this MemberInfo memberInfo, bool inherit, out TAttribute[] attributes)
            where TAttribute : Attribute
        {
            attributes = memberInfo.GetAllCustomAttributes<TAttribute>(inherit).ToArray();
            return attributes.Length > 0;
        }

        public static bool TryGetSequenceGroup<T>(
            this MemberInfo member,
            bool strict,
            out SequenceGroup<T> sequenceGroup)
                where T : unmanaged, Enum
        {
            if (member.TryGetAllCustomAttributes<SequenceGroupAttribute<T>>(true, out var sequenceAttributes))
            {
                sequenceGroup = sequenceAttributes.Single().Value;
                return true;
            }

            if (strict == true || member.TryGetAllCustomAttributes<RequireSequenceGroupAttribute<T>>(true, out var requiredSequenceAttributes))
            {
                throw new SequenceGroupException(typeof(T), member);
            }

            sequenceGroup = default;
            return false;
        }

        public static bool HasSequenceGroup<T>(this MemberInfo member)
            where T : unmanaged, Enum
        {
            return member.TryGetSequenceGroup<T>(false, out _);
        }
    }
}
