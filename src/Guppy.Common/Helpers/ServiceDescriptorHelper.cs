using Guppy.Common.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Helpers
{
    public static class ServiceDescriptorHelper
    {
        public static ServiceDescriptor Describe(Type serviceType, ServiceLifetime lifetime, Type? implementationType = null, Func<IServiceProvider, object>? factory = null, object? instance = null)
        {
            if (factory is not null)
            {
                return new ServiceDescriptor(serviceType, factory, lifetime);
            }

            if (instance is not null)
            {
                if (lifetime != ServiceLifetime.Singleton)
                {
                    throw new ArgumentException($"{nameof(ServiceDescriptorHelper)}::{nameof(Describe)} - {nameof(instance)} must not be defined unless {nameof(lifetime)} is {nameof(ServiceLifetime.Singleton)}. {lifetime} given.");
                }

                return new ServiceDescriptor(serviceType, instance);
            }

            if (implementationType is not null)
            {
                ThrowIf.Type.IsNotAssignableFrom(serviceType, implementationType);
                ThrowIf.Type.IsNotClass(implementationType);
            }

            return new ServiceDescriptor(serviceType, implementationType ?? serviceType, lifetime);
        }

        public static ServiceDescriptor Describe<T>(ServiceLifetime lifetime, Type? implementationType = null, Func<IServiceProvider, object>? factory = null, object? instance = null)
            where T : class
        {
            return Describe(typeof(T), lifetime, implementationType, factory, instance);
        }
    }
}
