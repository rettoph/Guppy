using Autofac;
using Guppy.Attributes;
using Guppy.Common.Providers;
using Guppy.Common.Utilities;
using Guppy.Configurations;
using Guppy.Loaders;
using Guppy.Providers;
using System.Reflection;

namespace Guppy
{
    public sealed class GuppyEngine
    {
        public IEnumerable<Assembly> Libraries { get; private set; }
        public IContainer Container { get; private set; }
        public IGuppyProvider Guppies { get; private set; }

        public GuppyStatus Status { get; private set; }

        public GuppyEngine(IEnumerable<Assembly>? libraries = default)
        {
            this.Status = GuppyStatus.NotReady;

            libraries ??= Enumerable.Empty<Assembly>();
            libraries = libraries.Concat(new[]
            {
                typeof(GuppyEngine).Assembly,
            });

            this.Libraries = libraries;
            this.Container = default!;
            this.Guppies = default!;
        }

        public GuppyEngine Start(
            Action<GuppyConfiguration>? build = null,
            Assembly? entry = null)
        {
            if(this.Status != GuppyStatus.NotReady)
            {
                throw new InvalidOperationException();
            }

            this.Status = GuppyStatus.Starting;

            entry ??= Assembly.GetEntryAssembly() ?? throw new NotImplementedException();
            var builder = new ContainerBuilder();
            var assemblies = new AssemblyProvider(this.Libraries);
            var configuration = new GuppyConfiguration(builder, assemblies);

            build?.Invoke(configuration);
            configuration.Build(entry);

            this.Container = builder.Build();
            this.Guppies = this.Container.Resolve<IGuppyProvider>();

            foreach(var loader in this.Container.Resolve<IEnumerable<IGlobalLoader>>())
            {
                loader.Load(this);
            }

            this.Status = GuppyStatus.Ready;

            return this;
        }
    }
}
