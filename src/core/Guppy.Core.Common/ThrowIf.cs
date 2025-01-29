using Guppy.Core.Common.Extensions.System;

namespace Guppy.Core.Common
{
    public static class ThrowIf
    {
        public static class Type
        {
            public static void IsNotUnmanagedStruct(System.Type type)
            {
                if (type.IsUnmanaged() == false)
                {
                    throw new ArgumentException($"'{type.FullName}' is not an unmanaged type.");
                }
            }

            public static bool IsNotAssignableFrom(System.Type to, System.Type from)
            {
                if (!to.IsAssignableFrom(from))
                {
                    throw new ArgumentException($"'{to.FullName}' is not assignable from '{from.FullName}'.");
                }

                return false;
            }

            public static bool IsNotAssignableFrom<TTo>(System.Type from)
            {
                return ThrowIf.Type.IsNotAssignableFrom(typeof(TTo), from);
            }

            public static void IsNotGenericTypeImplementation(System.Type genericTypeDefinition, System.Type implementation)
            {
                if (implementation.ImplementsGenericTypeDefinition(genericTypeDefinition))
                {
                    return;
                }

                throw new ArgumentException($"{nameof(implementation)} value of {implementation.Name} does not implement generic type definition {genericTypeDefinition.Name}.");
            }

            public static void IsNotGenericTypeDefinitionn(System.Type genericTypeDefinition)
            {
                if (!genericTypeDefinition.IsGenericTypeDefinition)
                {
                    throw new ArgumentException($"{nameof(genericTypeDefinition)} value of {genericTypeDefinition.Name} is not a valid Generic Type Definition.");
                }
            }

            public static void IsNotGenericType(System.Type genericType)
            {
                if (!genericType.IsGenericType)
                {
                    throw new ArgumentException($"{nameof(genericType)} value of {genericType.Name} is not a valid Generic Type.");
                }
            }

            public static void IsNotClass(System.Type type)
            {
                if (!type.IsClass)
                {
                    throw new ArgumentException($"'{type.FullName}' is not a class.");
                }
            }

            public static void IsAbstract(System.Type type)
            {
                if (type.IsAbstract)
                {
                    throw new ArgumentException($"'{type.FullName}' is abstract.");
                }
            }

            public static void IsAbstract<T>()
            {
                ThrowIf.Type.IsAbstract(typeof(T));
            }
        }
    }
}