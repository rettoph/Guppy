using Guppy.Attributes;
using Guppy.EntityComponent;
using Guppy.EntityComponent.Definitions;
using Guppy.Network.Attributes;
using Guppy.Network.Enums;
using Guppy.Settings;
using Guppy.Settings.Providers;
using Microsoft.Extensions.DependencyInjection;
using Minnow.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Definitions.ComponentFilters
{
    internal sealed class NetworkAuthorizationRequiredComponentFilter : ComponentFilterDefinition<IComponent>
    {
        private IAttributeProvider<IComponent, NetworkAuthorizationRequiredAttribute> _attributes;

        public NetworkAuthorizationRequiredComponentFilter(IAssemblyProvider assemblies)
        {
            _attributes = assemblies.GetAttributes<IComponent, NetworkAuthorizationRequiredAttribute>();
        }

        public override bool EntityFilter(IServiceProvider provider, IEntity entity, ComponentDefinition component)
        {
            var requiredNetworkAuthorization = _attributes[component.ComponentType].NetworkAuthorization;
            var currentNetworkAuthorization = provider.GetRequiredService<ISettingProvider>().Get<NetworkAuthorization>().Value;

            return requiredNetworkAuthorization == currentNetworkAuthorization;
        }

        public override bool TypeFilter(Type entity, ComponentDefinition component)
        {
            bool result = component.ComponentType.HasCustomAttribute<NetworkAuthorizationRequiredAttribute>();
            return result;
        }
    }
}
