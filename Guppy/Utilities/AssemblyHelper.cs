using Guppy.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Guppy.Utilities
{
    /// <summary>
    /// Simple helper class used to load all
    /// active assemplies or all loaded types
    /// </summary>
    public static class AssemblyHelper
    {
        public static HashSet<Assembly> Assemblies { get; private set; }
        public static HashSet<Type> Types { get; private set; }

        static AssemblyHelper()
        {
            AssemblyHelper.Assemblies = AssemblyHelper.GetUniqueNestedReferencedAssemblies(Assembly.GetEntryAssembly());
            AssemblyHelper.Types = new HashSet<Type>(
                AssemblyHelper.Assemblies.AsParallel().SelectMany(a => a.GetTypes()));
        }

        public static IEnumerable<Type> GetTypesWithAttribute<TAttribute>()
            where TAttribute : GuppyAttribute
        {
            return AssemblyHelper.Types.Where(t =>
            {
                var info = t.GetCustomAttributes(typeof(TAttribute), true);
                return info != null && info.Length > 0;
            })
            .OrderBy(t =>
            {
                var priority = (t.GetCustomAttribute(typeof(TAttribute)) as GuppyAttribute).Priority;
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
    }
}
