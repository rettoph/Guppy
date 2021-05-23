using Guppy.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Extensions.DependencyInjection
{
    public static class ServiceActionExtensions
    {
        public static Object Apply(this ServiceAction[] actions, Object instance, ServiceProvider provider, ServiceConfiguration configuration, Action<Object, ServiceProvider, ServiceConfiguration> setup = default, Int32 setupOrder = 0)
        {
            var ranCustom = setup == default;

            foreach (ServiceAction action in actions)
            {
                // Invoke the custom setup if neccessary...
                if (!ranCustom && action.Order > setupOrder && (ranCustom = true))
                    setup.Invoke(instance, provider, configuration);

                // Invoke the internal setup method
                action.Method(instance, provider, configuration);
            }

            if (!ranCustom) // Invoke the custom setup if neccessary...
                setup.Invoke(instance, provider, configuration);

            return instance;
        }
    }
}
