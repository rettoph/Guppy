using System.Collections;
using System.Reflection;
using Autofac;
using Guppy.Core.Common.Constants;
using Guppy.Core.Common.Extensions.System.Reflection;
using Guppy.Core.Common.Services;
using Guppy.Core.Common.Utilities;
using Guppy.Core.Logging.Common;
using Guppy.Core.Logging.Common.Extensions;

namespace Guppy.Core.Services
{
    public sealed class AssemblyService(IEnumerable<Assembly> libraries, ILogger logger) : IAssemblyService
    {
        private readonly ILogger _logger = logger;
        private readonly HashSet<Assembly> _assemblies = [];

        public AssemblyName[] Libraries { get; private set; } = libraries.Select(x => x.GetName()).Distinct().ToArray() ?? [];

        public event OnEventDelegate<IAssemblyService, Assembly>? OnAssemblyLoaded;

        public void Load(Assembly assembly, bool forced = false)
        {
            this.Load(assembly, forced, 0);
        }

        private void Load(Assembly assembly, bool forced, int depth)
        {
            if (!this.ShouldLoad(assembly, forced))
            {
                return;
            }

            this._assemblies.Add(assembly);
            this._logger.Information($"{new string('\t', depth)}Loading: {assembly.FullName}...");

            // Recersively attempt to load all references assemblies as well...
            foreach (AssemblyName referenceName in assembly.GetReferencedAssemblies())
            {
                //Debug.WriteLine($"{new string('\t', depth + 1)}Checking: {referenceName.FullName}");
                Assembly reference = Assembly.Load(referenceName);
                this.Load(reference, false, depth + 1);
            }

            this.OnAssemblyLoaded?.Invoke(this, assembly);
        }

        private bool ShouldLoad(Assembly assembly, bool forced)
        {
            if (forced)
            {
                return this._assemblies.Add(assembly);
            }

            if (this._assemblies.Contains(assembly))
            {
                return false;
            }

            if (this.Libraries.Length == 0)
            {
                return this._assemblies.Add(assembly);
            }

            if (this.Libraries.Any(r => AssemblyName.ReferenceMatchesDefinition(assembly.GetName(), r)))
            { // Check if the recieved assembly is an existing library...
                return this._assemblies.Add(assembly);
            }

            if (assembly.GetReferencedAssemblies().Any(nan => this.Libraries.Any(r => AssemblyName.ReferenceMatchesDefinition(nan, r))))
            { // Ensure the assembly references a required library...
                return this._assemblies.Add(assembly);
            }

            return false;
        }

        public ITypeService<T> GetTypes<T>()
        {
            return new TypeService<T>(this._assemblies.SelectMany(x => x.GetTypes()).Where(x => typeof(T).IsAssignableFrom(x)));
        }

        public ITypeService<T> GetTypes<T>(Func<Type, bool> predicate)
        {
            return new TypeService<T>(this._assemblies.SelectMany(x => x.GetTypes().Where(x => typeof(T).IsAssignableFrom(x)).Where(predicate)));
        }

        public IAttributeService<TType, TAttribute> GetAttributes<TType, TAttribute>(bool inherit)
            where TAttribute : Attribute
        {
            var types = this.GetTypes<TType>(x => x.TryGetAllCustomAttributes<TAttribute>(inherit, out _));
            return new AttributeService<TType, TAttribute>(types, inherit);
        }

        public IAttributeService<TType, TAttribute> GetAttributes<TType, TAttribute>(Func<Type, bool> predicate, bool inherit)
            where TAttribute : Attribute
        {
            var types = this.GetTypes<TType>(x => x.TryGetAllCustomAttributes<TAttribute>(inherit, out _) && predicate(x));
            return new AttributeService<TType, TAttribute>(types, inherit);
        }

        public IAttributeService<object, TAttribute> GetAttributes<TAttribute>(bool inherit)
            where TAttribute : Attribute
        {
            return this.GetAttributes<object, TAttribute>(inherit);
        }

        public IAttributeService<object, TAttribute> GetAttributes<TAttribute>(Func<Type, bool> predicate, bool inherit)
            where TAttribute : Attribute
        {
            return this.GetAttributes<object, TAttribute>(predicate, inherit);
        }

        public IEnumerator<Assembly> GetEnumerator()
        {
            return this._assemblies.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        internal static AssemblyService Factory(IComponentContext context)
        {
            GuppyEnvironment env = context.Resolve<GuppyEnvironment>();
            return new AssemblyService(env.Get<GuppyEnvironmentVariables.LibraryAssemblies>().Value, context.ResolveLogger<AssemblyService>());
        }
    }
}