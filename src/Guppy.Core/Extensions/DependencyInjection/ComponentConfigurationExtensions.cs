using Guppy.DependencyInjection;
using Guppy.DependencyInjection.ServiceConfigurations;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Extensions.DependencyInjection
{
    public static class ComponentConfigurationExtensions
    {
        public static IComponent[] Create(
            this ComponentConfiguration[] configurations,
            IEntity entity,
            ServiceProvider provider)
        {
            // TODO: Find way to make filter more efficient
            return configurations
                .Where(conf => conf.Validate(entity, provider))
                .Select(conf => conf.ComponentServiceConfiguration.GetInstance(
                        provider: provider, 
                        generics: default,
                        setup: (i, p, c) => (i as IComponent).Entity = entity,
                        setupOrder: Guppy.Core.Constants.Priorities.PreInitialize - 1
                    ) as IComponent
                )
                .ToArray();
        }
    }
}
