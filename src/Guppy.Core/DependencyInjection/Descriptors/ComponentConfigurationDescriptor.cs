using Guppy.Interfaces;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection.Descriptors
{
    public class ComponentConfigurationDescriptor
    {
        public readonly ServiceConfigurationKey ComponentServiceConfigurationKey;
        public readonly ServiceConfigurationKey EntityServiceConfigurationKey;

        public ComponentConfigurationDescriptor(
            ServiceConfigurationKey componentServiceConfigurationKey,
            ServiceConfigurationKey entityServicConfigurationeKey)
        {
            ExceptionHelper.ValidateAssignableFrom<IComponent>(componentServiceConfigurationKey.Type);
            ExceptionHelper.ValidateAssignableFrom<IEntity>(entityServicConfigurationeKey.Type);

            this.ComponentServiceConfigurationKey = componentServiceConfigurationKey;
            this.EntityServiceConfigurationKey = entityServicConfigurationeKey;
        }
    }
}
