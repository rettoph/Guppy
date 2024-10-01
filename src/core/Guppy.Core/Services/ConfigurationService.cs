using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Services;

namespace Guppy.Core.Services
{
    internal sealed class ConfigurationService(ILifetimeScope scope, IEnumerable<ConfigurationBuilder> builders) : IConfigurationService
    {
        private readonly ILifetimeScope _scope = scope;
        private readonly ConfigurationBuilder[] _builders = builders.ToArray();

        public void Configure<T>(T instance)
        {
            if (instance is null)
            {
                return;
            }

            foreach (ConfigurationBuilder builder in _builders)
            {
                if (builder.CanBuild(instance.GetType()))
                {
                    builder.Build(_scope, instance);
                }
            }
        }
    }
}
