using Guppy.EntityComponent.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Providers
{
    internal sealed class SetupProvider : ISetupProvider
    {
        private Dictionary<Type, SetupDescriptor[]> _configurations;

        public SetupProvider(Dictionary<Type, SetupDescriptor[]> configurations)
        {
            _configurations = configurations;
        }

        public ISetupService Create(IServiceProvider provider)
        {
            Dictionary<Type, ISetup[]> setups = _configurations.ToDictionary(
                keySelector: kvp => kvp.Key,
                elementSelector: kvp => kvp.Value.Select(sd => sd.Factory(provider)).ToArray());

            return new SetupService(setups);
        }
    }
}
