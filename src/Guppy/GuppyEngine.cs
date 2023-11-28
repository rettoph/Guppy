using Autofac;
using Guppy.Attributes;
using Guppy.Common;
using Guppy.Common.Extensions;
using Guppy.Common.Providers;
using Guppy.Common.Utilities;
using Guppy.Configurations;
using Guppy.Enums;
using Guppy.Loaders;
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
            Action<GuppyConfiguration>? build = null,
            Assembly? entry = null)
        {
            entry ??= Assembly.GetEntryAssembly() ?? throw new NotImplementedException();
            var container = GuppyConfiguration.Build(this.Environment, entry, this.Libraries, build);

            return container.Resolve<IGuppyProvider>();
        }
    }
}
