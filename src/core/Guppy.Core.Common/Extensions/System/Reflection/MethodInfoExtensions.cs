using System.Reflection;

namespace Guppy.Core.Common.Extensions.System.Reflection
{
    public static class MethodInfoExtensions
    {
        /// <summary>
        /// https://stackoverflow.com/questions/47741099/check-methodinfo-instance-is-implementation-of-interface-generic-method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool HasCustomAttributesIncludingInterfaces<T>(this MethodInfo methodInfo, bool inherit)
            where T : Attribute
        {
            if (inherit == false)
            {
                return methodInfo.HasCustomAttribute<T>();
            }

            if (methodInfo.HasCustomAttribute<T>())
            {
                return true;
            }

            foreach (Type implementedInterfaceType in methodInfo.DeclaringType!.GetInterfaces())
            {
                InterfaceMapping implementedInterfaceMapping = methodInfo.DeclaringType.GetInterfaceMap(implementedInterfaceType);
                if (implementedInterfaceMapping.TargetMethods.Contains(methodInfo) == false)
                {
                    continue;
                }

                Console.ReadLine();
            }

            return false;
        }

        /// <summary>
        /// https://stackoverflow.com/questions/47741099/check-methodinfo-instance-is-implementation-of-interface-generic-method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetCustomAttributesIncludingInterfaces<T>(this MethodInfo methodInfo, bool inherit)
            where T : Attribute
        {
            if (inherit == false)
            {
                return methodInfo.GetCustomAttributes<T>();
            }

            throw new NotImplementedException();
        }
    }
}