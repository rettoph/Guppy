using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public static class ThrowIf
    {
        public static class Type
        {
            public static void IsNotAssignableFrom(System.Type to, System.Type from)
            {
                if (!to.IsAssignableFrom(from))
                {
                    throw new ArgumentException($"'{to.FullName}' is not assignable from '{from.FullName}'.");
                }
            }

            public static void IsNotAssignableFrom<TTo>(System.Type from)
            {
                ThrowIf.Type.IsNotAssignableFrom(typeof(TTo), from);
            }

            public static void IsNotGenericTypeImplementation(System.Type genericTypeDefinition, System.Type implementation)
            {
                if(!genericTypeDefinition.IsGenericTypeDefinition)
                {
                    throw new ArgumentException($"{nameof(genericTypeDefinition)} value of {genericTypeDefinition.Name} is not a valid Generic Type Definition.");
                }

                foreach(var interfaceType in implementation.GetInterfaces())
                {
                    if(interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == genericTypeDefinition)
                    {
                        return;
                    }
                }

                throw new ArgumentException($"{nameof(implementation)} value of {implementation.Name} does not implement generic type definition {genericTypeDefinition.Name}.");
            }

            public static void IsNotClass(System.Type type)
            {
                if (!type.IsClass)
                {
                    throw new ArgumentException($"'{type.FullName}' is not a class.");
                }
            }
        }
    }
}
