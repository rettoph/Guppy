using Guppy.Attributes;
using Guppy.Common.Providers;
using Guppy.Common.Utilities;
using Guppy.Configurations;
using Guppy.Loaders;
using Guppy.Providers;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Guppy
{
    public sealed class GuppyEngine
    {
        public IEnumerable<Assembly> Libraries { get; private set; }
        public IServiceProvider Provider { get; private set; }
        public IGuppyProvider Guppies { get; private set; }

        public GuppyState State { get; private set; }

        public GuppyEngine(IEnumerable<Assembly>? libraries = default)
        {
            this.State = GuppyState.NotReady;

            libraries ??= Enumerable.Empty<Assembly>();
            libraries = libraries.Concat(new[]
            {
                typeof(GuppyEngine).Assembly,
            });

            this.Libraries = libraries;
            this.Provider = default!;
            this.Guppies = default!;
        }

        public GuppyEngine Start(
            Action<GuppyConfiguration>? build = null,
            Assembly? entry = null)
        {
            if(this.State != GuppyState.NotReady)
            {
                throw new InvalidOperationException();
            }

            this.State = GuppyState.Starting;

            entry ??= Assembly.GetEntryAssembly() ?? throw new NotImplementedException();
            var services = new ServiceCollection();
            var assemblies = new AssemblyProvider(this.Libraries);
            var configuration = new GuppyConfiguration(services, assemblies);

            build?.Invoke(configuration);
            configuration.Build(entry);

            this.Provider = services.BuildServiceProvider();
            this.Guppies = this.Provider.GetRequiredService<IGuppyProvider>();

            foreach(var loader in this.Provider.GetServices<IGlobalLoader>())
            {
                loader.Load(this);
            }

            this.State = GuppyState.Ready;

            return this;
        }
    }
}
