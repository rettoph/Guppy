using System.Reflection;

namespace Guppy.Core.Common.Extensions.System
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

        public static IEnumerable<Type> GetAncestors<TParent>(this Type child) => child.GetAncestors(typeof(TParent));

        public static IEnumerable<Type> AssignableFrom(this IEnumerable<Type> types, Type parent) => types.Where(parent.IsAssignableFrom);

        public static IEnumerable<Type> AssignableFrom<TParent>(this IEnumerable<Type> types) => types.AssignableFrom(typeof(TParent));

        public static IEnumerable<Type> WithAttribute(this IEnumerable<Type> types, Type attribute, bool inherit)
        {
            ThrowIf.Type.IsNotAssignableFrom<Attribute>(attribute);

            return types.Where(t =>
            {
                object[] info = t.GetCustomAttributes(attribute, inherit);
                return info != null && info.Length > 0;
            });
        }

        public static IEnumerable<Type> WithAttribute<TAttribute>(this IEnumerable<Type> types, bool inherit)
            where TAttribute : Attribute => types.WithAttribute(typeof(TAttribute), inherit);

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

            if (genericTypeDefinition.IsInterface)
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

            while (currentType is not null)
            {
                if (currentType.IsGenericType)
                {
                    if (currentType.GetGenericTypeDefinition() == genericTypeDefinition)
                    {
                        return true;
                    }
                }

                currentType = currentType.BaseType;
            }

            return false;
        }

        /// <summary>
        /// Returns the type name. If this is a generic type, appends
        /// the list of generic type arguments between angle brackets.
        /// (Does not account for embedded / inner generic arguments.)
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>System.String.</returns>
        public static string GetFormattedName(this Type type, bool includeAssembly = false)
        {
            string name = (includeAssembly ? type.AssemblyQualifiedName : type.Name) ?? type.Name;
            if (type.IsGenericType)
            {
                string genericArguments = type.GetGenericArguments()
                                    .Select(x => x.Name)
                                    .Aggregate((x1, x2) => $"{x1}, {x2}");
                return $"{name[..name.IndexOf('`')]}"
                     + $"<{genericArguments}>";
            }

            return name;
        }

        public static bool IsUnmanaged(this Type type)
        {
            // primitive, pointer or enum -> true
            if (type.IsPrimitive || type.IsPointer || type.IsEnum)
            {
                return true;
            }

            // not a struct -> false
            if (!type.IsValueType)
            {
                return false;
            }

            // otherwise check recursively
            return type
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .All(f => IsUnmanaged(f.FieldType));
        }
    }
}