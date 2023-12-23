using Autofac;
using Guppy.Common;
using Guppy.Providers;
using System.Reflection;

namespace Guppy
{
    public sealed class GuppyEngine
    {
        public IEnumerable<Assembly> Libraries { get; private set; }

        public IGuppyEnvironment Environment { get; private set; }

        public GuppyEngine(string company, string name, IEnumerable<Assembly>? libraries = default)
        {
            this.Environment = new GuppyEnvironment()
            {
                Company = company,
                Name = name
            };

            libraries ??= Enumerable.Empty<Assembly>();
            libraries = libraries.Concat(new[]
            {
                typeof(GuppyEngine).Assembly,
            });

            this.Libraries = libraries;
        }

        public IGuppyProvider Start(
            Action<ContainerBuilder>? build = null,
            Assembly? entry = null)
        {
            entry ??= Assembly.GetEntryAssembly() ?? throw new NotImplementedException();

            var container = GuppyBuilder.Build(this.Environment, entry, this.Libraries, build);

            return container.Resolve<IGuppyProvider>();
        }
    }
}
