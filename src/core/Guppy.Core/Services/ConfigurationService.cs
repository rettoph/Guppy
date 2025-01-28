using Guppy.Core.Common;
using Guppy.Core.Common.Configurations;
using Guppy.Core.Common.Services;

namespace Guppy.Core.Services
{
    internal sealed class ConfigurationService(IGuppyScope scope, IEnumerable<Configurator> configurators) : IConfigurationService
    {
        private readonly IGuppyScope _scope = scope;
        private readonly Configurator[] _configurators = configurators.ToArray();

        public void Configure<T>(T instance)
        {
            if (instance is null)
            {
                return;
            }

            foreach (Configurator configurator in this._configurators)
            {
                if (configurator.CanBuild(instance.GetType()))
                {
                    configurator.Configure(this._scope, instance);
                }
            }
        }
    }
}