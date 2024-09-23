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
        public static TAttribute[] GetAllCustomAttributes<TAttribute>(this MemberInfo memberInfo, bool inherit)
            where TAttribute : Attribute
        {
            List<TAttribute> result = new List<TAttribute>();

            IEnumerable<TAttribute> memberAttributes = memberInfo.GetCustomAttributes(inherit).OfType<TAttribute>();
            result.AddRange(memberAttributes);

            if (inherit == false)
            { // No interface checking needed
                return result.ToArray();
            }

            if (memberInfo is Type type)
            {
                IEnumerable<TAttribute> typeInterfaceAttributes = type.GetInterfaces().SelectMany(x => x.GetCustomAttributes(true).OfType<TAttribute>());
                result.AddRange(typeInterfaceAttributes);
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
                    result.AddRange(mappedInterfaceMethodInfo.GetCustomAttributes(inherit).OfType<TAttribute>());
                }
            }

            return result.ToArray();
        }

        public static bool TryGetAllCustomAttributes<TAttribute>(this MemberInfo memberInfo, bool inherit, out TAttribute[] attributes)
            where TAttribute : Attribute
        {
            attributes = memberInfo.GetAllCustomAttributes<TAttribute>(inherit);
            return attributes.Length > 0;
        }

        public static IEnumerable<TSequenceGroup> GetSequenceGroups<TSequenceGroup>(
            this MemberInfo member,
            bool strict,
            TSequenceGroup? defaultSequenceGroup = null)
            where TSequenceGroup : unmanaged, Enum
        {
            if (member.TryGetAllCustomAttributes<SequenceGroupAttribute<TSequenceGroup>>(true, out var sequenceAttributes))
            {
                return member.GetCustomAttributes<SequenceGroupAttribute<TSequenceGroup>>().Select(x => x.Value);
            }

            if (strict == true || member.TryGetAllCustomAttributes<RequireSequenceGroupAttribute<TSequenceGroup>>(true, out var requiredSequenceAttributes))
            {
                throw new SequenceException(typeof(TSequenceGroup), member);
            }

            if (defaultSequenceGroup is not null)
            {
                return defaultSequenceGroup.Value.Yield();
            }

            return Enumerable.Empty<TSequenceGroup>();
        }
    }
}
