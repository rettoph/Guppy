using Autofac;
using Autofac.Features.ResolveAnything;
using Guppy.Attributes;
using Guppy.Common;
using Guppy.Common.Autofac;
using Guppy.Common.Providers;
using Guppy.Common.Utilities;
using Guppy.Extensions.Autofac;
using Guppy.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    internal sealed class GuppyBuilder
    {
        public readonly IAssemblyProvider Assemblies;
        public readonly IGuppyEnvironment Environment;
        public readonly ContainerBuilder ContainerBuilder;

        private GuppyBuilder(
            IGuppyEnvironment environment,
            Assembly entry,
            IEnumerable<Assembly> libraries,
            Action<ContainerBuilder>? build)
        {
            Assemblies = new AssemblyProvider(libraries);
            Environment = environment;

            ContainerBuilder = new ContainerBuilder();
            ContainerBuilder.RegisterInstance(Assemblies);
            ContainerBuilder.RegisterInstance(Environment);

            Assemblies.OnAssemblyLoaded += HandleAssemblyLoaded;
            Assemblies.Load(entry);
            Assemblies.OnAssemblyLoaded -= HandleAssemblyLoaded;

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
    }
}
