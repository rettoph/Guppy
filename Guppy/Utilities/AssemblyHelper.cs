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
            AssemblyHelper.Assemblies = AssemblyHelper.GetUniqueNestedReferencedAssemblies(Assembly.GetEntryAssembly());
            AssemblyHelper.Types = new HashSet<Type>(
                AssemblyHelper.Assemblies.AsParallel().SelectMany(a => a.GetTypes()));
        }
        #endregion

        #region Helper Methods
        public static IEnumerable<Type> GetTypesAssignableFrom<TBAse>()
        {
            return AssemblyHelper.GetTypesAssignableFrom(typeof(TBAse));
        }
        public static IEnumerable<Type> GetTypesAssignableFrom(Type baseType)
        {
            return AssemblyHelper.Types
                .Where(t => baseType.IsAssignableFrom(t));
        }

        public static IEnumerable<Type> GetTypesWithAttribute<TBase, TAttribute>(Boolean inherit = true)
            where TAttribute : GuppyAttribute
        {
            return AssemblyHelper.GetTypesWithAttribute(typeof(TBase), typeof(TAttribute), inherit);
        }
        public static IEnumerable<Type> GetTypesWithAttribute(Type baseType, Type attribute, Boolean inherit = true)
        {
            if (!typeof(GuppyAttribute).IsAssignableFrom(attribute))
                throw new Exception("Unable to load types with attribute, attribute type does not extend GuppyAttribute.");

            return AssemblyHelper.GetTypesAssignableFrom(baseType)
                .Where(t => {
                    var info = t.GetCustomAttributes(attribute, inherit);
                    return info != null && info.Length > 0;
                })
                .OrderBy(t =>
                {
                    var priority = (t.GetCustomAttribute(attribute) as GuppyAttribute).Priority;
                    return priority;
                });
        }

        /// <summary>
        /// Return a list of all referenced assemblies
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="set"></param>
        /// <returns></returns>
        private static HashSet<Assembly> GetUniqueNestedReferencedAssemblies(Assembly entry, HashSet<Assembly> set = null)
        {
            if (set == null)
                set = new HashSet<Assembly>();

            if (set.Add(entry))
                foreach (Assembly child in entry.GetReferencedAssemblies().AsParallel().Select(an => Assembly.Load(an)))
                    AssemblyHelper.GetUniqueNestedReferencedAssemblies(child, set);

            return set;
        }
        #endregion
    }
}
