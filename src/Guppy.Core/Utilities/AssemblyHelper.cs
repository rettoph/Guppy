using Guppy.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Guppy.Utilities
{
    /// <summary>
    /// Simple static helper class used to interact
    /// with all loaded assemblies and their types.
    /// 
    /// This is required to dynamically load and configure
    /// games, scenes, layers, and even service loaders.
    /// </summary>
    public static class AssemblyHelper
    {
        /// <summary>
        /// List of all unique assemblies loaded
        /// </summary>
        public static HashSet<Assembly> Assemblies { get; private set; }
        /// <summary>
        /// List of all unique types loaded
        /// </summary>
        public static HashSet<Type> Types { get; private set; }

        #region Constructor
        static AssemblyHelper()
        {
            AssemblyHelper.Assemblies = AssemblyHelper.GetUniqueNestedReferencedAssemblies(Assembly.GetEntryAssembly(), Assembly.GetExecutingAssembly().GetName());
            AssemblyHelper.Types = new HashSet<Type>(
                AssemblyHelper.Assemblies.AsParallel().SelectMany(a => a.GetTypes()));

            var test = Assemblies;
        }
        #endregion

        #region Helper Methods
        public static IEnumerable<Type> GetTypesAssignableFrom<TBase>()
        {
            return AssemblyHelper.GetTypesAssignableFrom(typeof(TBase));
        }
        public static IEnumerable<Type> GetTypesAssignableFrom(Type baseType)
        {
            return AssemblyHelper.Types
                .Where(t => baseType.IsAssignableFrom(t) || (baseType.IsGenericType && AssemblyHelper.IsSubclassOfRawGeneric(baseType, t)));
        }

        public static IEnumerable<Type> GetTypesWithAttribute<TBase, TAttribute>(Boolean inherit = true)
            where TAttribute : Attribute
        {
            return AssemblyHelper.GetTypesWithAttribute(typeof(TBase), typeof(TAttribute), inherit);
        }
        public static IEnumerable<Type> GetTypesWithAttribute(Type baseType, Type attribute, Boolean inherit = true)
        {
            if (!typeof(Attribute).IsAssignableFrom(attribute))
                throw new Exception("Unable to load types with attribute, attribute type does not extend Attribute.");

            return AssemblyHelper.GetTypesAssignableFrom(baseType)
                .Where(t =>
                {
                    var info = t.GetCustomAttributes(attribute, inherit);
                    return info != null && info.Length > 0;
                });
        }
        public static IEnumerable<Type> GetTypesWithAutoLoadAttribute(Type type, Boolean inherit = true, Type autoLoadAttribute = null)
        {
            autoLoadAttribute = autoLoadAttribute ?? typeof(AutoLoadAttribute);
            ExceptionHelper.ValidateAssignableFrom<AutoLoadAttribute>(autoLoadAttribute);

            return AssemblyHelper.GetTypesWithAttribute(type, autoLoadAttribute, inherit).OrderBy(t =>
            {
                return t.GetCustomAttributes(autoLoadAttribute, inherit).Min(attr => (attr as AutoLoadAttribute).Priority);
            });
        }
        public static IEnumerable<Type> GetTypesWithAutoLoadAttribute<T>(Boolean inherit = true)
        {
            return AssemblyHelper.GetTypesWithAutoLoadAttribute(typeof(T), inherit);
        }
        public static IEnumerable<Type> GetTypesWithAutoLoadAttribute<T, TAutoLoadAttribute>(Boolean inherit = true)
            where TAutoLoadAttribute : AutoLoadAttribute
        {
            return AssemblyHelper.GetTypesWithAutoLoadAttribute(typeof(T), inherit, typeof(TAutoLoadAttribute));
        }

        /// <summary>
        /// Return a list of all referenced assemblies
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="set"></param>
        /// <returns></returns>
        private static HashSet<Assembly> GetUniqueNestedReferencedAssemblies(Assembly entry, AssemblyName executing, HashSet<Assembly> set = null)
        {
            if (set == null)
                set = new HashSet<Assembly>();

            var test = entry.GetReferencedAssemblies();


            if (set.Add(entry))
                foreach (Assembly child in entry.GetReferencedAssemblies().Where(an => AssemblyName.ReferenceMatchesDefinition(an, executing) || Assembly.Load(an).GetReferencedAssemblies().Any(nan => AssemblyName.ReferenceMatchesDefinition(nan, executing))).Select(an => Assembly.Load(an)))
                    AssemblyHelper.GetUniqueNestedReferencedAssemblies(child, executing, set);

            return set;
        }

        /// <summary>
        /// As advertised, stolen from here:
        /// https://stackoverflow.com/questions/457676/check-if-a-class-is-derived-from-a-generic-class
        /// </summary>
        /// <param name="generic"></param>
        /// <param name="toCheck"></param>
        /// <returns></returns>
        public static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }
        #endregion
    }
}
