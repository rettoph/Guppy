﻿using Guppy.Attributes;
using Guppy;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using Guppy.Initializers;
using Guppy.Loaders;
using Minnow.Providers;

namespace Guppy
{
    public sealed class GuppyEngine
    {
        private List<IGuppyInitializer> _initializers;
        private List<IGuppyLoader> _loaders;

        public IAssemblyProvider Assemblies { get; private init; }

        public HashSet<string> Tags { get; private set; }

        public GuppyEngine(
            Assembly? entry = null, 
            IEnumerable<Assembly>? libraries = null)
        {
            libraries ??= Enumerable.Empty<Assembly>();
            libraries = libraries.Concat(new[]
            {
                typeof(IGuppyInitializer).Assembly,
            });

            _initializers = new List<IGuppyInitializer>();
            _loaders = new List<IGuppyLoader>();

            this.Assemblies = new AssemblyProvider(libraries);
            this.Assemblies.OnAssemblyLoaded += this.HandleAssemblyLoaded;

            this.Assemblies.Load(entry ?? Assembly.GetEntryAssembly() ?? throw new InvalidOperationException());

            this.Tags = new HashSet<string>();
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

            foreach(IGuppyInitializer initializer in _initializers)
            {
                initializer.Initialize(this.Assemblies, services, _loaders);
            }

            return this;
        }

        public IServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();

            this.ConfigureServices(services);

            return services.BuildServiceProvider();
        }

        private void HandleAssemblyLoaded(IAssemblyProvider sender, Assembly assembly)
        {
            assembly.GetTypes().GetTypesWithAttribute<IGuppyInitializer, AutoLoadAttribute>()
                .OrderBy(t => t.GetAttribute<AutoLoadAttribute>()?.Order ?? 0)
                .Select(t => Activator.CreateInstance(t) as IGuppyInitializer)
                .ForEach(i => this.AddInitializer(i!));

            assembly.GetTypes().GetTypesWithAttribute<IGuppyLoader, AutoLoadAttribute>()
                .OrderBy(t => t.GetAttribute<AutoLoadAttribute>()?.Order ?? 0)
                .Select(t => Activator.CreateInstance(t) as IGuppyLoader)
                .ForEach(l => this.AddLoader(l!));
        }
    }
}