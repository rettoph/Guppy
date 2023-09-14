using Autofac;
using Guppy.Attributes;
using Guppy.Common;
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
        private readonly List<IGuppyConfigurator> _configurators;

        public readonly ContainerBuilder Builder;
        public readonly IAssemblyProvider Assemblies;
        public readonly IGuppyEnvironment Environment;

        public IEnumerable<Action<GuppyConfiguration>> Loaders => _loaders.Values;

        internal GuppyConfiguration(string company, string name, ContainerBuilder builder, IAssemblyProvider assemblies)
        {
            _loaders = new SortedList<int, Action<GuppyConfiguration>>(new DuplicateKeyComparer<int>());
            _configurators = new List<IGuppyConfigurator>();

            Builder = builder;
            Assemblies = assemblies;
            Environment = new GuppyEnvironment()
            {
                Company = company,
                Name = name
            };

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

            foreach (IGuppyConfigurator configurator in _configurators)
            {
                configurator.Configure(this);
            }

            this.Builder.RegisterInstance(Assemblies);
            this.Builder.RegisterInstance(Environment);

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
                _configurators.Add(configurator);
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
