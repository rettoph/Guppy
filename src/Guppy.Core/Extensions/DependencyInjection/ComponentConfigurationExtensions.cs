using Guppy.DependencyInjection;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Extensions.DependencyInjection
{
    public static class ComponentConfigurationExtensions
    {
        public static IEnumerable<IComponent> Create(
            this ComponentConfiguration[] configurations,
            IEntity entity,
            GuppyServiceProvider provider)
        {
            foreach(ComponentConfiguration configuration in configurations)
            {
                if(configuration.CheckFilters(entity, provider))
                {
                    IComponent component = configuration.ComponentServiceConfiguration.GetInstance(
                        provider: provider,
                        customSetup: (i, p, c) => (i as IComponent).Entity = entity,
                        customSetupOrder: Guppy.Core.Constants.Priorities.PreInitialize - 1
                    ) as IComponent;

                    yield return component;
                }
            }
        }
    }
}
