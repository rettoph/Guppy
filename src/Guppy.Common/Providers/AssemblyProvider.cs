using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Providers
{
    public sealed class AssemblyProvider : IAssemblyProvider
    {
        private readonly HashSet<Assembly> _assemblies;

        public AssemblyName[] Libraries { get; private set; }

        public event OnEventDelegate<IAssemblyProvider, Assembly>? OnAssemblyLoaded;

        public AssemblyProvider(IEnumerable<Assembly>? libraries)
        {
            _assemblies = new HashSet<Assembly>();

            this.Libraries = libraries?.Select(x => x.GetName()).Distinct().ToArray() ?? Array.Empty<AssemblyName>();
        }

        public void Load(Assembly assembly, bool forced = false)
        {
            if(!this.ShouldLoad(assembly, forced))
            {
                return;
            }

            Debug.WriteLine($"Preparing to load: {assembly.FullName}");

            // Recersively attempt to load all references assemblies as well...
            foreach (Assembly reference in assembly.GetReferencedAssemblies().Select(an => Assembly.Load(an)))
            {
                this.Load(reference);
            }

            this.OnAssemblyLoaded?.Invoke(this, assembly);
        }

        private bool ShouldLoad(Assembly assembly, bool forced)
        {
            if(forced)
            {
                return _assemblies.Add(assembly);
            }

            if (this.Libraries.Length == 0)
            {
                return _assemblies.Add(assembly);
            }

            if (this.Libraries.Any(r => AssemblyName.ReferenceMatchesDefinition(assembly.GetName(), r)))
            { // Check if the recieved assembly is an existing library...
                return _assemblies.Add(assembly);
            }

            if (assembly.GetReferencedAssemblies().Any(nan => this.Libraries.Any(r => AssemblyName.ReferenceMatchesDefinition(nan, r))))
            { // Ensure the assembly references a required library...
                return _assemblies.Add(assembly);
            }

            return false;
        }

        public ITypeProvider<T> GetTypes<T>()
        {
            return new TypeProvider<T>(_assemblies.SelectMany(x => x.GetTypes()).Where(x => typeof(T).IsAssignableFrom(x)));
        }

        public ITypeProvider<T> GetTypes<T>(Func<Type, bool> predicate)
        {
            return new TypeProvider<T>(_assemblies.SelectMany(x => x.GetTypes().Where(x => typeof(T).IsAssignableFrom(x)).Where(predicate)));
        }

        public IAttributeProvider<TType, TAttribute> GetAttributes<TType, TAttribute>(bool inherit)
            where TAttribute : Attribute
        {
            var types = this.GetTypes<TType>(x => x.HasCustomAttribute<TAttribute>(inherit));
            return new AttributeProvider<TType, TAttribute>(types);
        }

        public IAttributeProvider<TType, TAttribute> GetAttributes<TType, TAttribute>(Func<Type, bool> predicate, bool inherit)
            where TAttribute : Attribute
        {
            var types = this.GetTypes<TType>(x => x.HasCustomAttribute<TAttribute>(inherit) && predicate(x));
            return new AttributeProvider<TType, TAttribute>(types);
        }

        public IAttributeProvider<object, TAttribute> GetAttributes<TAttribute>(bool inherit)
            where TAttribute : Attribute
        {
            return this.GetAttributes<object, TAttribute>(inherit);
        }

        public IAttributeProvider<object, TAttribute> GetAttributes<TAttribute>(Func<Type, bool> predicate, bool inherit)
            where TAttribute : Attribute
        {
            return this.GetAttributes<object, TAttribute>(predicate, inherit);
        }

        public IEnumerator<Assembly> GetEnumerator()
        {
            return _assemblies.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
