using Guppy.Attributes;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Extensions.System
{
    public static class TypeExtensions
    {
        public static IEnumerable<Type> GetTypesAssignableFrom<TBase>(this IEnumerable<Type> types)
        {
            return types.GetTypesAssignableFrom(typeof(TBase));
        }
        public static IEnumerable<Type> GetTypesAssignableFrom(this IEnumerable<Type> types, Type baseType)
            => types.Where(t => baseType.IsAssignableFrom(t) || (baseType.IsGenericType && t.IsSubclassOfRawGeneric(baseType)));

        /// <summary>
        /// As advertised, stolen from here:
        /// https://stackoverflow.com/questions/457676/check-if-a-class-is-derived-from-a-generic-class
        /// </summary>
        /// <param name="type"></param>
        /// <param name="generic"></param>
        /// <returns></returns>
        public static bool IsSubclassOfRawGeneric(this Type type, Type generic)
        {
            while (type != null && type != typeof(object))
            {
                var cur = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
                if (generic == cur)
                {
                    return true;
                }
                type = type.BaseType;
            }
            return false;
        }

        public static IEnumerable<Type> GetTypesWithAttribute<TBase, TAttribute>(this IEnumerable<Type> types, Boolean inherit = true)
            where TAttribute : Attribute
                => types.GetTypesWithAttribute(typeof(TBase), typeof(TAttribute), inherit);

        public static IEnumerable<Type> GetTypesWithAttribute(this IEnumerable<Type> types, Type baseType, Type attribute, Boolean inherit = true)
        {
            if (!typeof(Attribute).IsAssignableFrom(attribute))
                throw new Exception("Unable to load types with attribute, attribute type does not extend Attribute.");

            return types.GetTypesAssignableFrom(baseType)
                .Where(t =>
                {
                    var info = t.GetCustomAttributes(attribute, inherit);
                    return info != null && info.Length > 0;
                });
        }
        public static IEnumerable<Type> GetTypesWithAutoLoadAttribute(this IEnumerable<Type> types, Type type, Boolean inherit = true, Type autoLoadAttribute = null)
        {
            autoLoadAttribute = autoLoadAttribute ?? typeof(AutoLoadAttribute);
            ExceptionHelper.ValidateAssignableFrom<AutoLoadAttribute>(autoLoadAttribute);

            return types.GetTypesWithAttribute(type, autoLoadAttribute, inherit).OrderBy(t =>
            {
                return t.GetCustomAttributes(autoLoadAttribute, inherit).Min(attr => (attr as AutoLoadAttribute).Priority);
            });
        }
        public static IEnumerable<Type> GetTypesWithAutoLoadAttribute<T>(this IEnumerable<Type> types, Boolean inherit = true)
            => types.GetTypesWithAutoLoadAttribute(typeof(T), inherit);

        public static IEnumerable<Type> GetTypesWithAutoLoadAttribute<T, TAutoLoadAttribute>(this IEnumerable<Type> types, Boolean inherit = true)
            where TAutoLoadAttribute : AutoLoadAttribute
                => types.GetTypesWithAutoLoadAttribute(typeof(T), inherit, typeof(TAutoLoadAttribute));

        /// <summary>
        /// https://stackoverflow.com/questions/17480990/get-name-of-generic-class-without-tilde
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string GetPrettyName(this Type t)
        {
            if (!t.IsGenericType)
                return t.Name;

            StringBuilder sb = new StringBuilder();
            sb.Append(t.Name.Substring(0, t.Name.IndexOf('`')));
            sb.Append('<');
            bool appendComma = false;
            foreach (Type arg in t.GetGenericArguments())
            {
                if (appendComma) sb.Append(',');
                sb.Append(GetPrettyName(arg));
                appendComma = true;
            }
            sb.Append('>');
            return sb.ToString();
        }

        /// <summary>
        /// Recursively return all <see cref="Type"/> ancestors
        /// between a given child and parent type (inclusive)
        /// </summary>
        /// <param name="child"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetAncestors(this Type child, Type parent)
        {
            var type = child;
            ExceptionHelper.ValidateAssignableFrom(parent, child);

            if(child == parent)
            {
                yield return child;
            }
            else if (parent.IsInterface)
            { // Recersively add types until the interface is no longer implemented...
                while (type.GetInterfaces().Contains(parent))
                {
                    yield return type;
                    type = type.BaseType;
                }

                yield return parent;
            }
            else
            { // Add types until the base type is hit...
                while (type != parent)
                {
                    yield return type;
                    type = type.BaseType;
                }

                yield return parent;
            }
        }
        public static IEnumerable<Type> GetAncestors<TParent>(this Type child)
            => child.GetAncestors(typeof(TParent));
    }
}
