using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Providers
{
    public sealed class AssemblyProvider : IAssemblyProvider
    {
        private HashSet<Assembly> _assemblies;

        public AssemblyName[] Libraries { get; private set; }

        public event OnEventDelegate<IAssemblyProvider, Assembly>? OnAssemblyLoaded;

        public AssemblyProvider(IEnumerable<Assembly>? libraries)
        {
            _assemblies = new HashSet<Assembly>();

            this.Libraries = libraries?.Select(x => x.GetName()).Distinct().ToArray() ?? Array.Empty<AssemblyName>();
        }

        public void Load(Assembly assembly)
        {
            if(!this.ShouldLoad(assembly) || !_assemblies.Add(assembly))
            {
                return;
            }

            // Recersively attempt to load all references assemblies as well...
            foreach (Assembly reference in assembly.GetReferencedAssemblies().Select(an => Assembly.Load(an)))
            {
                this.Load(reference);
            }

            this.OnAssemblyLoaded?.Invoke(this, assembly);
        }

        private bool ShouldLoad(Assembly assembly)
        {
            if (this.Libraries.Count() == 0)
            {
                return true;
            }

            if (this.Libraries.Any(r => AssemblyName.ReferenceMatchesDefinition(assembly.GetName(), r)))
            { // Check if the recieved assembly is an existing library...
                return true;
            }

            if (assembly.GetReferencedAssemblies().Any(nan => this.Libraries.Any(r => AssemblyName.ReferenceMatchesDefinition(nan, r))))
            { // Ensure the assembly references a required library...
                return true;
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

        public IAttributeProvider<TType, TAttribute> GetAttributes<TType, TAttribute>()
            where TAttribute : Attribute
        {
            var types = this.GetTypes<TType>(x => x.HasCustomAttribute<TAttribute>());
            return new AttributeProvider<TType, TAttribute>(types);
        }

        public IAttributeProvider<TType, TAttribute> GetAttributes<TType, TAttribute>(Func<Type, bool> predicate)
            where TAttribute : Attribute
        {
            var types = this.GetTypes<TType>(x => predicate(x) && x.HasCustomAttribute<TAttribute>());
            return new AttributeProvider<TType, TAttribute>(types);
        }

        public IAttributeProvider<object, TAttribute> GetAttributes<TAttribute>()
            where TAttribute : Attribute
        {
            return this.GetAttributes<object, TAttribute>();
        }

        public IAttributeProvider<object, TAttribute> GetAttributes<TAttribute>(Func<Type, bool> predicate)
            where TAttribute : Attribute
        {
            return this.GetAttributes<object, TAttribute>(predicate);
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
