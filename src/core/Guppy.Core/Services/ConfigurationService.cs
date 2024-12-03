using Autofac;
using Guppy.Core.Common.Configurations;
using Guppy.Core.Common.Services;

namespace Guppy.Core.Services
{
    internal sealed class ConfigurationService(ILifetimeScope scope, IEnumerable<ServiceConfiguration> builders) : IConfigurationService
    {
        private readonly ILifetimeScope _scope = scope;
        private readonly ServiceConfiguration[] _configurators = builders.ToArray();

        public void Configure<T>(T instance)
        {
            if (instance is null)
            {
                return;
            }

            foreach (ServiceConfiguration configurator in _configurators)
            {
                if (configurator.CanBuild(instance.GetType()))
                {
                    configurator.Configure(_scope, instance);
                }
            }
        }
    }
}
