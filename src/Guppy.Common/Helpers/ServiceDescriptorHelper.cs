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
            int nullPropertyCount = implementationType is null ? 0 : 1;
            nullPropertyCount += factory is null ? 0 : 1;
            nullPropertyCount += instance is null ? 0 : 1;

            if (nullPropertyCount > 1)
            {
                throw new ArgumentException($"{nameof(ServiceDescriptorHelper)}::{nameof(Describe)} - Only up to 1 of the following arguments may not be null: '{nameof(implementationType)}', '{nameof(factory)}', and '{nameof(instance)}'. Found {nullPropertyCount}.");
            }

            if (instance is not null && lifetime != ServiceLifetime.Singleton)
            {
                throw new ArgumentException($"{nameof(ServiceDescriptorHelper)}::{nameof(Describe)} - {nameof(instance)} must not be defined unless {nameof(lifetime)} is {nameof(ServiceLifetime.Singleton)}. {lifetime} given.");
            }

            if (implementationType is not null)
            {
                ThrowIf.Type.IsNotAssignableFrom(implementationType, serviceType);
                ThrowIf.Type.IsNotClass(serviceType);
            }

            if (factory is not null)
            {
                return new ServiceDescriptor(serviceType, factory, lifetime);
            }

            if (instance is not null)
            {
                return new ServiceDescriptor(serviceType, instance);
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
