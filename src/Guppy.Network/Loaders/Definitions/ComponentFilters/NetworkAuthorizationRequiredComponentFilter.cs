using Guppy.Attributes;
using Guppy.EntityComponent;
using Guppy.EntityComponent.Loaders.Definitions;
using Guppy.EntityComponent.Loaders.Descriptors;
using Guppy.Network.Attributes;
using Guppy.Network.Enums;
using Guppy.Settings;
using Guppy.Settings.Providers;
using Microsoft.Extensions.DependencyInjection;
using Minnow.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Loaders.Definitions.ComponentFilters
{
    internal sealed class NetworkAuthorizationRequiredComponentFilter : ComponentFilterDefinition<IComponent>
    {
        private static AttributeCache<NetworkAuthorizationRequiredAttribute> Attributes = new AttributeCache<NetworkAuthorizationRequiredAttribute>();

        public override bool EntityFilter(IEntity entity, IServiceProvider provider, ComponentDescriptor descriptor)
        {
            var requiredNetworkAuthorization = Attributes[descriptor.ComponentType].NetworkAuthorization;
            var currentNetworkAuthorization = provider.GetRequiredService<ISettingProvider>().Get<NetworkAuthorization>().Value;

            return requiredNetworkAuthorization == currentNetworkAuthorization;
        }

        public override bool TypeFilter(Type entity, ComponentDescriptor descriptor)
        {
            bool result = descriptor.ComponentType.HasCustomAttribute<NetworkAuthorizationRequiredAttribute>();
            return result;
        }
    }
}
