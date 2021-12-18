using Guppy.Attributes;
using Guppy.EntityComponent.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class TypeExtensions
    {
        public static IEnumerable<Type> GetTypesWithAutoLoadAttribute(this IEnumerable<Type> types, Type type, Boolean inherit = true, Type autoLoadAttribute = null)
        {
            autoLoadAttribute = autoLoadAttribute ?? typeof(AutoLoadAttribute);
            typeof(AutoLoadAttribute).ValidateAssignableFrom(autoLoadAttribute);

            return types.GetTypesWithAttribute(type, autoLoadAttribute, inherit).OrderBy(t =>
            {
                return t.GetCustomAttributes(autoLoadAttribute, inherit).Min(attr => (attr as AutoLoadAttribute).Order);
            });
        }
        public static IEnumerable<Type> GetTypesWithAutoLoadAttribute<T>(this IEnumerable<Type> types, Boolean inherit = true)
            => types.GetTypesWithAutoLoadAttribute(typeof(T), inherit);

        public static IEnumerable<Type> GetTypesWithAutoLoadAttribute<T, TAutoLoadAttribute>(this IEnumerable<Type> types, Boolean inherit = true)
            where TAutoLoadAttribute : AutoLoadAttribute
                => types.GetTypesWithAutoLoadAttribute(typeof(T), inherit, typeof(TAutoLoadAttribute));
    }
}
