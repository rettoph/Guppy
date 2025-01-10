using System.Reflection;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Exceptions;
using Guppy.Core.Common.Interfaces;

namespace Guppy.Core.Common.Extensions.System.Reflection
{
    public static class MemberInfoExtensions
    {
        public static bool HasCustomAttribute<T>(this MemberInfo member)
            where T : Attribute => member.GetCustomAttributes<T>().Any();

        public static bool HasCustomAttribute<T>(this MemberInfo member, bool inherit)
             where T : Attribute => member.GetCustomAttributes(inherit).Any(x => x is T);

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
            object? instance,
            bool strict,
            out SequenceGroup<T> sequenceGroup)
                where T : unmanaged, Enum
        {
            if (instance is not null && member.DeclaringType is not null && member.DeclaringType.IsInterface == true)
            {
                Type instanceType = instance.GetType();
                InterfaceMapping interfaceMapping = instanceType.GetInterfaceMap(member.DeclaringType);
                int index = Array.IndexOf(interfaceMapping.InterfaceMethods, member);
                if (index == -1)
                {
                    throw new SequenceGroupException(typeof(T), member);
                }

                MemberInfo targetMember = interfaceMapping.TargetMethods[index];
                if (targetMember == member)
                { // This should never happen.. but just in case...
                    throw new SequenceGroupException(typeof(T), member);
                }

                return targetMember.TryGetSequenceGroup<T>(instance, strict, out sequenceGroup);
            }

            if (member.TryGetAllCustomAttributes<SequenceGroupAttribute<T>>(true, out var sequenceGroupAttributes))
            {
                sequenceGroup = sequenceGroupAttributes.Single().Value;
                return true;
            }

            if (instance is not null and IRuntimeSequenceGroup<T> runtimeSequenceGroup)
            {
                sequenceGroup = runtimeSequenceGroup.Value;
                return true;
            }

            if (strict && member.TryGetAllCustomAttributes<RequireSequenceGroupAttribute<T>>(true, out _))
            {
                throw new SequenceGroupException(typeof(T), member);
            }

            sequenceGroup = default;
            return false;
        }

        public static SequenceGroup<T> GetSequenceGroup<T>(
            this MemberInfo member,
            object? instance)
                where T : unmanaged, Enum
        {
            if (member.TryGetSequenceGroup<T>(instance, true, out var sequenceGroup) == true)
            {
                return sequenceGroup;
            }

            throw new KeyNotFoundException();
        }

        public static bool HasSequenceGroup<T>(this MemberInfo member, object? instance)
            where T : unmanaged, Enum => member.TryGetSequenceGroup<T>(instance, false, out _);

        public static int GetSequence<T>(
            this MemberInfo member,
            object? instance)
                where T : unmanaged, Enum
        {
            if (member.TryGetAllCustomAttributes<SequenceAttribute<T>>(true, out var sequenceGroupOrderAttributes))
            {
                return sequenceGroupOrderAttributes.Single().Value;
            }

            if (instance is not null and IRuntimeSequence<T> runtimeSequenceGroupOrder)
            {
                return runtimeSequenceGroupOrder.Value;
            }

            // Default order
            return 0;
        }

        public static bool HasSequence<T>(this MemberInfo member, object? instance)
            where T : unmanaged, Enum
        {
            if (member.HasSequenceGroup<T>(instance) == false)
            {
                return false;
            }

            if (member.TryGetAllCustomAttributes<SequenceAttribute<T>>(true, out _))
            {
                return true;
            }

            if (instance is not null and IRuntimeSequence<T>)
            {
                return true;
            }

            return false;
        }
    }
}