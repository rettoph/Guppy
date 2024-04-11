using Autofac;
using Guppy.Attributes;
using Guppy.Common;
using Guppy.Common.Autofac;
using Guppy.Common.Services;
using Guppy.Extensions.Autofac;
using Guppy.Loaders;
using System.Reflection;

namespace Guppy
{
    internal sealed class GuppyBuilder
    {
        public readonly IAssemblyService Assemblies;
        public readonly IGuppyEnvironment Environment;
        public readonly ContainerBuilder ContainerBuilder;

        private GuppyBuilder(
            IGuppyEnvironment environment,
            Assembly entry,
            IEnumerable<Assembly> libraries,
            Action<ContainerBuilder>? build)
        {
            this.Assemblies = new AssemblyService(libraries);
            this.Environment = environment;

            this.ContainerBuilder = new ContainerBuilder();
            this.ContainerBuilder.RegisterInstance(Assemblies);
            this.ContainerBuilder.RegisterInstance(Environment);

            this.Assemblies.OnAssemblyLoaded += HandleAssemblyLoaded;
            this.Assemblies.Load(entry);
            this.Assemblies.OnAssemblyLoaded -= HandleAssemblyLoaded;

            var guppyConfigurationAttributes = Assemblies.GetAttributes<GuppyConfigurationAttribute>(true);
            foreach ((Type type, GuppyConfigurationAttribute[] attributes) in guppyConfigurationAttributes)
            {
                foreach (GuppyConfigurationAttribute attribute in attributes)
                {
                    attribute.TryConfigure(ContainerBuilder, type);
                }
            }

            build?.Invoke(ContainerBuilder);
        }

        private ILifetimeScope Build()
        {
            return ContainerBuilder.Build().BeginGuppyScope(LifetimeScopeTags.MainScope);
        }

        internal static ILifetimeScope Build(
            IGuppyEnvironment environment,
            Assembly entry,
            IEnumerable<Assembly> libraries,
            Action<ContainerBuilder>? build = null)
        {
            var configuration = new GuppyBuilder(environment, entry, libraries, build);

            return configuration.Build();
        }

        private void HandleAssemblyLoaded(IAssemblyService sender, Assembly assembly)
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
    }
}
