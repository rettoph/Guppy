using Autofac;
using Autofac.Features.ResolveAnything;
using Guppy.Attributes;
using Guppy.Common;
using Guppy.Common.Providers;
using Guppy.Common.Utilities;
using Guppy.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Configurations
{
    public sealed class GuppyConfiguration
    {
        private readonly IContainer _serviceLoaderContainer;

        public readonly IAssemblyProvider Assemblies;
        public readonly IGuppyEnvironment Environment;
        public readonly ContainerBuilder Builder;
        public readonly List<IServiceLoader> ServiceLoaders;

        private GuppyConfiguration(
            IGuppyEnvironment environment,
            Assembly entry,
            IEnumerable<Assembly> libraries,
            Action<GuppyConfiguration>? build)
        {
            this.ServiceLoaders = new List<IServiceLoader>();
            this.Assemblies = new AssemblyProvider(libraries);
            this.Environment = environment;
            this.Builder = new ContainerBuilder();
            this.Builder.RegisterInstance(this.Assemblies);
            this.Builder.RegisterInstance(this.Environment);

            var builder = new ContainerBuilder();
            builder.RegisterInstance(this.Assemblies);
            builder.RegisterInstance(this.Environment);
            builder.RegisterInstance(this);
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
            _serviceLoaderContainer = builder.Build();

            this.Assemblies.OnAssemblyLoaded += HandleAssemblyLoaded;
            this.Assemblies.Load(entry);
            this.Assemblies.OnAssemblyLoaded -= HandleAssemblyLoaded;

            build?.Invoke(this);
            var guppyConfigurationAttributes = this.Assemblies.GetAttributes<GuppyConfigurationAttribute>(true);
            foreach ((Type type, GuppyConfigurationAttribute[] attributes) in guppyConfigurationAttributes)
            {
                foreach(GuppyConfigurationAttribute attribute in attributes)
                {
                    attribute.TryConfigure(this, type);
                }
            }

            foreach(IServiceLoader loader in this.ServiceLoaders)
            {
                loader.ConfigureServices(this.Builder);
            }
        }

        internal static IContainer Build(
            IGuppyEnvironment environment,
            Assembly entry,
            IEnumerable<Assembly> libraries,
            Action<GuppyConfiguration>? build = null)
        {
            var configuration = new GuppyConfiguration(environment, entry, libraries, build);

            return configuration.Builder.Build();
        }

        private void HandleAssemblyLoaded(IAssemblyProvider sender, Assembly assembly)
        {
            // First initialize all assembly loaders
            var loaders = assembly.GetTypes()
                .AssignableFrom<IAssemblyLoader>()
                .WithAttribute<AutoLoadAttribute>(true)
                .Select(t => Activator.CreateInstance(t) as IAssemblyLoader ?? throw new Exception());

            foreach (IAssemblyLoader loader in loaders)
            {
                loader.ConfigureAssemblies(sender);
            }
        }

        public GuppyConfiguration AddServiceLoader(IServiceLoader loader)
        {
            this.ServiceLoaders.Add(loader);

            return this;
        }

        public GuppyConfiguration AddServiceLoader(Type type)
        {
            ThrowIf.Type.IsNotAssignableFrom<IServiceLoader>(type);

            var loader = _serviceLoaderContainer.Resolve(type) as IServiceLoader ?? throw new Exception();
            return this.AddServiceLoader(loader);
        }

        public GuppyConfiguration AddServiceLoader<TLoader>()
            where TLoader : IServiceLoader
        {
            var loader = _serviceLoaderContainer.Resolve<TLoader>();
            return this.AddServiceLoader(loader);
        }
    }
}
