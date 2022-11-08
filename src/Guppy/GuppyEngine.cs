using Guppy.Attributes;
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
        private readonly List<IGuppyInitializer> _initializers;
        private readonly List<IGuppyLoader> _loaders;
        private bool _initialized;

        public IAssemblyProvider Assemblies { get; private init; }
        public HashSet<string> Tags { get; private set; }

        public GuppyEngine(IEnumerable<Assembly>? libraries = default)
        {
            _initializers = new List<IGuppyInitializer>();
            _loaders = new List<IGuppyLoader>();
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

        public GuppyEngine AddInitializer(IGuppyInitializer initializer)
        {
            _initializers.Add(initializer);

            return this;
        }

        public GuppyEngine AddLoader(IGuppyLoader loader)
        {
            _loaders.Add(loader);

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
                .OrderBy(t => t.GetCustomAttribute<AutoLoadAttribute>()!.Order)
                .Select(t => Activator.CreateInstance(t) as IGuppyInitializer ?? throw new Exception());

            var loaders = assembly.GetTypes()
                .AssignableFrom<IGuppyLoader>()
                .WithAttribute<AutoLoadAttribute>(true)
                .OrderBy(t => t.GetCustomAttribute<AutoLoadAttribute>()!.Order)
                .Select(t => Activator.CreateInstance(t) as IGuppyLoader ?? throw new Exception());

            foreach(IGuppyInitializer initializer in initializers)
            {
                this.AddInitializer(initializer);
            }

            foreach (IGuppyLoader loader in loaders)
            {
                this.AddLoader(loader);
            }
        }
    }
}
