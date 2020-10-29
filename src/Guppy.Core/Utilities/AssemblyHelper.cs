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
        }
        #endregion

        #region Helper Methods
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
        #endregion
    }
}
