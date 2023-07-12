﻿using Autofac;
using Guppy.Attributes;
using Guppy.Common.Providers;
using Guppy.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Configurations
{
    public sealed class GuppyConfiguration : IDisposable
    {
        private SortedList<int, Action<GuppyConfiguration>> _loaders;

        public readonly ContainerBuilder Builder;
        public readonly IAssemblyProvider Assemblies;

        public IEnumerable<Action<GuppyConfiguration>> Loaders => _loaders.Values;

        internal GuppyConfiguration(ContainerBuilder builder, IAssemblyProvider assemblies)
        {
            _loaders = new SortedList<int, Action<GuppyConfiguration>>(new DuplicateKeyComparer<int>());

            Builder = builder;
            Assemblies = assemblies;

            Assemblies.OnAssemblyLoaded += HandleAssemblyLoaded;
        }

        public void Dispose()
        {
            Assemblies.OnAssemblyLoaded -= HandleAssemblyLoaded;
        }

        public GuppyConfiguration AddLoader(Action<GuppyConfiguration> loader, int? order = null)
        {
            _loaders.Add(order ?? 0, loader);

            return this;
        }

        internal GuppyConfiguration Build(Assembly entry)
        {
            Assemblies.Load(entry);

            // Invoke all loaders
            foreach (var loader in _loaders)
            {
                loader.Value.Invoke(this);
            }

            this.Builder.RegisterInstance(Assemblies);

            return this;
        }

        private void HandleAssemblyLoaded(IAssemblyProvider sender, Assembly assembly)
        {
            // First initialize all auto load initializers
            var configurators = assembly.GetTypes()
                .AssignableFrom<IGuppyConfigurator>()
                .WithAttribute<AutoLoadAttribute>(true)
                .Select(t => Activator.CreateInstance(t) as IGuppyConfigurator ?? throw new Exception());

            foreach (IGuppyConfigurator configurator in configurators)
            {
                configurator.Configure(this);
            }

            // Initialize all initializable attributes
            var typesWithInitializableAttributes = assembly.GetTypes()
                .Select(x => (x, x.GetCustomAttributesIncludingInterfaces<GuppyConfigurationAttribute>().ToArray()))
                .Where(x => x.Item2.Any());

            foreach ((Type type, GuppyConfigurationAttribute[] attributes) in typesWithInitializableAttributes)
            {
                foreach (var attribute in attributes)
                {
                    attribute.TryConfigure(this, type);
                }
            }
        }
    }
}
