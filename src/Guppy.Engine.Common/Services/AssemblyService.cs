using Guppy.Engine.Common.Contexts;
using System.Collections;
using System.Reflection;

namespace Guppy.Engine.Common.Services
{
    public sealed class AssemblyService : IAssemblyService
    {
        private readonly HashSet<Assembly> _assemblies;

        public AssemblyName[] Libraries { get; private set; }

        public event OnEventDelegate<IAssemblyService, Assembly>? OnAssemblyLoaded;

        public AssemblyService(IGuppyContext context)
        {
            _assemblies = new HashSet<Assembly>();

            Libraries = context.Libraries.Select(x => x.GetName()).Distinct().ToArray() ?? Array.Empty<AssemblyName>();
        }

        public void Load(Assembly assembly, bool forced = false)
        {
            Load(assembly, forced, 0);
        }

        private void Load(Assembly assembly, bool forced, int depth)
        {
            if (!ShouldLoad(assembly, forced))
            {
                return;
            }

            _assemblies.Add(assembly);
            Console.WriteLine($"{new string('\t', depth)}Loading: {assembly.FullName}...");

            // Recersively attempt to load all references assemblies as well...
            foreach (AssemblyName referenceName in assembly.GetReferencedAssemblies())
            {
                //Debug.WriteLine($"{new string('\t', depth + 1)}Checking: {referenceName.FullName}");
                Assembly reference = Assembly.Load(referenceName);
                Load(reference, false, depth + 1);
            }

            OnAssemblyLoaded?.Invoke(this, assembly);
        }

        private bool ShouldLoad(Assembly assembly, bool forced)
        {
            if (forced)
            {
                return _assemblies.Add(assembly);
            }

            if (_assemblies.Contains(assembly))
            {
                return false;
            }

            if (Libraries.Length == 0)
            {
                return _assemblies.Add(assembly);
            }

            if (Libraries.Any(r => AssemblyName.ReferenceMatchesDefinition(assembly.GetName(), r)))
            { // Check if the recieved assembly is an existing library...
                return _assemblies.Add(assembly);
            }

            if (assembly.GetReferencedAssemblies().Any(nan => Libraries.Any(r => AssemblyName.ReferenceMatchesDefinition(nan, r))))
            { // Ensure the assembly references a required library...
                return _assemblies.Add(assembly);
            }

            return false;
        }

        public ITypeService<T> GetTypes<T>()
        {
            return new TypeService<T>(_assemblies.SelectMany(x => x.GetTypes()).Where(x => typeof(T).IsAssignableFrom(x)));
        }

        public ITypeService<T> GetTypes<T>(Func<Type, bool> predicate)
        {
            return new TypeService<T>(_assemblies.SelectMany(x => x.GetTypes().Where(x => typeof(T).IsAssignableFrom(x)).Where(predicate)));
        }

        public IAttributeService<TType, TAttribute> GetAttributes<TType, TAttribute>(bool inherit)
            where TAttribute : Attribute
        {
            var types = GetTypes<TType>(x => x.HasCustomAttributesIncludingInterfaces<TAttribute>(inherit));
            return new AttributeService<TType, TAttribute>(types, inherit);
        }

        public IAttributeService<TType, TAttribute> GetAttributes<TType, TAttribute>(Func<Type, bool> predicate, bool inherit)
            where TAttribute : Attribute
        {
            var types = GetTypes<TType>(x => x.HasCustomAttributesIncludingInterfaces<TAttribute>(inherit) && predicate(x));
            return new AttributeService<TType, TAttribute>(types, inherit);
        }

        public IAttributeService<object, TAttribute> GetAttributes<TAttribute>(bool inherit)
            where TAttribute : Attribute
        {
            return GetAttributes<object, TAttribute>(inherit);
        }

        public IAttributeService<object, TAttribute> GetAttributes<TAttribute>(Func<Type, bool> predicate, bool inherit)
            where TAttribute : Attribute
        {
            return GetAttributes<object, TAttribute>(predicate, inherit);
        }

        public IEnumerator<Assembly> GetEnumerator()
        {
            return _assemblies.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
