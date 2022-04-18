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
    internal sealed class HostTypeRequiredComponentFilter : ComponentFilterDefinition<IComponent>
    {
        private readonly IAttributeProvider<IComponent, HostTypeRequiredAttribute> _attributes;

        public HostTypeRequiredComponentFilter(IAssemblyProvider assemblies)
        {
            _attributes = assemblies.GetAttributes<IComponent, HostTypeRequiredAttribute>();
        }

        public override bool EntityFilter(IServiceProvider provider, IEntity entity, ComponentDefinition component)
        {
            var requiredHostType = _attributes[component.ComponentType].HostType;
            var currentHostType = provider.GetRequiredService<ISettingProvider>().Get<HostType>().Value;

            return requiredHostType == currentHostType;
        }

        public override bool TypeFilter(Type entity, ComponentDefinition component)
        {
            return component.ComponentType.HasCustomAttribute<HostTypeRequiredAttribute>();
        }
    }
}
