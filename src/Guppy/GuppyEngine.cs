using Autofac;
using Guppy.Attributes;
using Guppy.Common;
using Guppy.Common.Providers;
using Guppy.Common.Utilities;
using Guppy.Configurations;
using Guppy.Loaders;
using Guppy.Providers;
using System.Reflection;

namespace Guppy
{
    public sealed class GuppyEngine : IDisposable
    {
        private IContainer _container;

        public IEnumerable<Assembly> Libraries { get; private set; }
        public IGuppyProvider Guppies { get; private set; }

        public GuppyStatus Status { get; private set; }

        public IGuppyEnvironment Environment { get; private set; }

        public GuppyEngine(string company, string name, IEnumerable<Assembly>? libraries = default)
        {
            this.Status = GuppyStatus.NotReady;
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

            _container = default!;
            this.Libraries = libraries;
            this.Guppies = default!;
        }

        public IContainer Start(
            Action<GuppyConfiguration>? build = null,
            Assembly? entry = null)
        {
            if(this.Status != GuppyStatus.NotReady)
            {
                throw new InvalidOperationException();
            }

            this.Status = GuppyStatus.Starting;

            entry ??= Assembly.GetEntryAssembly() ?? throw new NotImplementedException();
            _container = GuppyConfiguration.Build(this.Environment, entry, this.Libraries, build);

            this.Guppies = _container.Resolve<IGuppyProvider>();

            this.Status = GuppyStatus.Ready;

            return _container;
        }

        public void Dispose()
        {
            this.Guppies.Dispose();
        }
    }
}
