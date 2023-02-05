using Guppy.Attributes;
using Guppy.Common.Providers;
using Guppy.Common.Utilities;
using Guppy.Loaders;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Guppy
{
    public sealed class GuppyEngine
    {
        private bool _loaded;
        private SortedList<int, Action<GuppyEngine>> _loaders;

        public IAssemblyProvider Assemblies { get; private set; }
        public IServiceCollection Services { get; private set; }

        public GuppyEngine(IEnumerable<Assembly>? libraries = default)
        {
            _loaded = false;
            _loaders = new SortedList<int, Action<GuppyEngine>>(new DuplicateKeyComparer<int>());

            libraries ??= Enumerable.Empty<Assembly>();
            libraries = libraries.Concat(new[]
            {
                typeof(GuppyEngine).Assembly,
            });

            this.Services = new ServiceCollection();
            this.Assemblies = new AssemblyProvider(libraries);

            this.Assemblies.OnAssemblyLoaded += this.HandleAssemblyLoaded;
        }

        public GuppyEngine AddLoader(Action<GuppyEngine> loader, int? order = null)
        {
            _loaders.Add(order ?? loader.GetOrder(), loader);

            return this;
        }

        public GuppyEngine Load(Assembly? entry = null)
        {
            if(_loaded)
            {
                throw new InvalidOperationException();
            }

            this.Services.AddSingleton<IAssemblyProvider>(this.Assemblies);

            this.Assemblies.Load(entry ?? Assembly.GetEntryAssembly() ?? throw new InvalidOperationException());

            // Invoke all loaders
            foreach(var loader in _loaders)
            {
                loader.Value.Invoke(this);
            }

            this.Services.RefreshManagers();

            _loaded = true;

            return this;
        }

        public IServiceProvider Build()
        {
            if(!_loaded)
            {
                this.Load();
            }

            var provider = this.Services.BuildServiceProvider(validateScopes: true);

            return provider;
        }

        private void HandleAssemblyLoaded(IAssemblyProvider sender, Assembly assembly)
        {
            // First initialize all auto load initializers
            var initialize = assembly.GetTypes()
                .AssignableFrom<IGuppyInitializer>()
                .WithAttribute<AutoLoadAttribute>(true)
                .Select(t => Activator.CreateInstance(t) as IGuppyInitializer ?? throw new Exception());

            foreach (IGuppyInitializer initializer in initialize)
            {
                initializer.Initialize(this);
            }

            // Initialize all initializable attributes
            var typesWithInitializableAttributes = assembly.GetTypes()
                .Select(x => (x, x.GetCustomAttributesIncludingInterfaces<InitializableAttribute>().ToArray()))
                .Where(x => x.Item2.Any());

            foreach ((Type type, InitializableAttribute[] attributes) in typesWithInitializableAttributes)
            {
                foreach (var attribute in attributes)
                {
                    attribute.TryInitialize(this, type);
                }
            }
        }
    }
}
