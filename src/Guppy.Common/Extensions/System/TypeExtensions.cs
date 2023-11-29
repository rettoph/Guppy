using Guppy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Recursively return all <see cref="Type"/> ancestors
        /// between a given child and parent type (inclusive)
        /// </summary>
        /// <param name="child"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetAncestors(this Type child, Type parent)
        {
            ThrowIf.Type.IsNotAssignableFrom(parent, child);

            Type? type = child;

            if (child == parent)
            {
                yield return child;
            }
            if (parent.IsInterface)
            { // Recersively add types until the interface is no longer implemented...
                while (type is not null && type.GetInterfaces().Contains(parent))
                {
                    yield return type;
                    type = type.BaseType;
                }

                yield return parent;
            }
            else
            { // return types until the base type is hit...
                while (parent.IsAssignableFrom(type))
                {
                    yield return type;
                    type = type.BaseType;
                }
            }
        }

        public static IEnumerable<Type> GetAncestors<TParent>(this Type child)
        {
            return child.GetAncestors(typeof(TParent));
        }

        public static IEnumerable<Type> AssignableFrom(this IEnumerable<Type> types, Type parent)
        {
            return types.Where(t => parent.IsAssignableFrom(t));
        }

        public static IEnumerable<Type> AssignableFrom<TParent>(this IEnumerable<Type> types)
        {
            return types.AssignableFrom(typeof(TParent));
        }

        public static IEnumerable<Type> WithAttribute(this IEnumerable<Type> types, Type attribute, bool inherit)
        {
            ThrowIf.Type.IsNotAssignableFrom<Attribute>(attribute);

            return types.Where(t =>
            {
                var info = t.GetCustomAttributes(attribute, inherit);
                return info != null && info.Length > 0;
            });
        }

        public static IEnumerable<Type> WithAttribute<TAttribute>(this IEnumerable<Type> types, bool inherit)
            where TAttribute : Attribute
        {
            return types.WithAttribute(typeof(TAttribute), inherit);
        }

        /// <summary>
        /// https://stackoverflow.com/questions/540749/can-a-c-sharp-class-inherit-attributes-from-its-interface
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetCustomAttributesIncludingInterfaces<T>(this Type type, bool inherit)
            where T : Attribute
        {
            if (inherit == false)
            {
                return type.GetCustomAttributes<T>();
            }

            var attributeType = typeof(T);
            return type.GetCustomAttributes(attributeType, true)
              .Union(type.GetInterfaces().SelectMany(interfaceType =>
                  interfaceType.GetCustomAttributes(attributeType, true)))
              .Cast<T>();
        }

        /// <summary>
        /// https://stackoverflow.com/questions/540749/can-a-c-sharp-class-inherit-attributes-from-its-interface
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool HasCustomAttributesIncludingInterfaces<T>(this Type type, bool inherit)
            where T : Attribute
        {
            if(inherit == false)
            {
                return type.HasCustomAttribute<T>();
            }

            if(type.HasCustomAttribute<T>())
            {
                return true;

            }

            foreach(Type interfaceType in type.GetInterfaces())
            {
                if(interfaceType.HasCustomAttribute<T>())
                {
                    return true;
                }
            }

            return false;
        }

        public static IEnumerable<Type> GetConstructedGenericTypes(this Type type, Type genericTypeDefinition)
        {
            var interfaceTypes = type.GetInterfaces();

            foreach (var interfaceType in interfaceTypes)
            {
                if (interfaceType.IsConstructedGenericType && interfaceType.GetGenericTypeDefinition() == genericTypeDefinition)
                {
                    yield return interfaceType;
                }
            }
        }

        public static bool ImplementsGenericTypeDefinition(this Type type, Type genericTypeDefinition)
        {
            if (!genericTypeDefinition.IsGenericTypeDefinition)
            {
                throw new ArgumentException($"{nameof(genericTypeDefinition)} value of {genericTypeDefinition.Name} is not a valid Generic Type Definition.");
            }

            if(genericTypeDefinition.IsInterface)
            {
                foreach (var interfaceType in type.GetInterfaces())
                {
                    if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == genericTypeDefinition)
                    {
                        return true;
                    }
                }

                return false;
            }

            Type? currentType = type;

            while(currentType is not null)
            {
                if(currentType.IsGenericType)
                {
                    if(currentType.GetGenericTypeDefinition() == genericTypeDefinition)
                    {
                        return true;
                    }
                }

                currentType = currentType.BaseType;
            }

            return false;
        }
    }
}
