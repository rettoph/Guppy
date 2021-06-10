using Guppy.DependencyInjection;
using Guppy.Extensions.System;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceCollection = Guppy.DependencyInjection.ServiceCollection;
using ServiceProvider = Guppy.DependencyInjection.ServiceProvider;

namespace Guppy.Extensions.DependencyInjection
{
    public static class ServiceDescriptorExtensions
    {
        /// <summary>
        /// Parse an incoming service descriptor instance &
        /// convert it into a useable Guppy TypeFactory/ServiceConfiguration
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="services"></param>
        internal static void ConvertServiceDescriptor(this ServiceDescriptor descriptor, ServiceCollection services)
        {
            var implementationType = descriptor.ImplementationType ?? descriptor.ServiceType;
            var key = ServiceConfigurationKey.From(descriptor.ServiceType);
            var count = services.ServiceConfigurations.Count(c => c.Key == key);

            services.RegisterTypeFactory(
                type: implementationType,
                typeImplementation: implementationType, 
                method: descriptor.CreateFactoryMethod(),
                priority: count);

            services.RegisterServiceConfiguration(
                key,
                descriptor.Lifetime,
                implementationType,
                ServiceConfigurationKey.From(implementationType).Yield(),
                priority: count);
        }

        internal static Func<ServiceProvider, Type, Object> CreateFactoryMethod(this ServiceDescriptor descriptor)
        {
            if (descriptor.ImplementationInstance != default)
                return (p, t) => descriptor.ImplementationInstance;
            else if (descriptor.ImplementationFactory != default)
                return (p, t) => descriptor.ImplementationFactory(p);
            else
                return (p, t) => ActivatorUtilities.CreateInstance(p, t);
        }
    }
}
