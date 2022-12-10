using Guppy.Attributes;
using Guppy.Common;
using Guppy.Common.Providers;
using Guppy.Initializers;
using Guppy.Loaders;
using Guppy.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    public sealed class GuppyEngine
    {
        private readonly OrderedList<IGuppyInitializer> _initializers;
        private readonly OrderedList<IGuppyLoader> _loaders;
        private bool _initialized;

        public IAssemblyProvider Assemblies { get; private init; }
        public HashSet<string> Tags { get; private set; }

        public GuppyEngine(IEnumerable<Assembly>? libraries = default)
        {
            _initializers = new OrderedList<IGuppyInitializer>();
            _loaders = new OrderedList<IGuppyLoader>();
            _initialized = false;

            libraries ??= Enumerable.Empty<Assembly>();
            libraries = libraries.Concat(new[]
            {
                typeof(GuppyEngine).Assembly,
            });

            this.Tags = new HashSet<string>();

            this.Assemblies = new AssemblyProvider(libraries);
            this.Assemblies.OnAssemblyLoaded += this.HandleAssemblyLoaded;
        }

        public GuppyEngine Initialize(Assembly? entry = null)
        {
            this.Assemblies.Load(entry ?? Assembly.GetEntryAssembly() ?? throw new InvalidOperationException());

            _initialized = true;

            return this;
        }

        public GuppyEngine AddInitializer(IGuppyInitializer initializer, int order)
        {
            _initializers.Add(initializer, order);

            return this;
        }

        public GuppyEngine AddLoader(IGuppyLoader loader, int order)
        {
            _loaders.Add(loader, order);

            return this;
        }

        public GuppyEngine AddTag(string tag)
        {
            this.Tags.Add(tag);

            return this;
        }

        public GuppyEngine ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(this.Assemblies);

            foreach (IGuppyInitializer initializer in _initializers)
            {
                initializer.Initialize(this.Assemblies, services, _loaders);
            }

            services.RefreshManagers();

            return this;
        }

        public IServiceProvider Build()
        {
            if(!_initialized)
            {
                this.Initialize();
            }

            var services = new ServiceCollection();

            this.ConfigureServices(services);

            var provider = services.BuildServiceProvider(validateScopes: true);

            return provider;
        }

        private void HandleAssemblyLoaded(IAssemblyProvider sender, Assembly assembly)
        {
            var initializers = assembly.GetTypes()
                .AssignableFrom<IGuppyInitializer>()
                .WithAttribute<AutoLoadAttribute>(true)
                .Select(t => Activator.CreateInstance(t) as IGuppyInitializer ?? throw new Exception());

            var loaders = assembly.GetTypes()
                .AssignableFrom<IGuppyLoader>()
                .WithAttribute<AutoLoadAttribute>(true)
                .Select(t => Activator.CreateInstance(t) as IGuppyLoader ?? throw new Exception());

            foreach(IGuppyInitializer initializer in initializers)
            {
                this.AddInitializer(initializer, initializer.GetType()?.GetCustomAttribute<AutoLoadAttribute>(true)?.Order ?? 0);
            }

            foreach (IGuppyLoader loader in loaders)
            {
                this.AddLoader(loader, loader.GetType()?.GetCustomAttribute<AutoLoadAttribute>(true)?.Order ?? 0);
            }
        }
    }
}
