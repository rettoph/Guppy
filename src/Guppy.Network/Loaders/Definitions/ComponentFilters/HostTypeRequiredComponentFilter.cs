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
    internal sealed class HostTypeRequiredComponentFilter : ComponentFilterDefinition<IComponent>
    {
        private static AttributeCache<HostTypeRequiredAttribute> Attributes = new AttributeCache<HostTypeRequiredAttribute>();

        public override bool EntityFilter(IEntity entity, IServiceProvider provider, ComponentDescriptor descriptor)
        {
            var requiredHostType = Attributes[descriptor.ComponentType].HostType;
            var currentHostType = provider.GetRequiredService<ISettingProvider>().Get<HostType>().Value;

            return requiredHostType == currentHostType;
        }

        public override bool TypeFilter(Type entity, ComponentDescriptor descriptor)
        {
            return descriptor.ComponentType.HasCustomAttribute<HostTypeRequiredAttribute>();
        }
    }
}
