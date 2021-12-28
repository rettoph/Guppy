using Minnow.General.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.EntityComponent.DependencyInjection.Builders.Interfaces
{
    public interface ServiceConfigurationBuilder : IPrioritizable
    {
        String Name { get; }

        /// <summary>
        /// The lookup key of the service's <see cref="TypeFactory"/>.
        /// </summary>
        public Type FactoryType { get; }

        /// <summary>
        /// The service lifetime.
        /// </summary>
        public ServiceLifetime Lifetime { get; }

        /// <summary>
        /// A list of strings with which this service will be cached once activated.
        /// All queries matching any of these values will return the defined
        /// configuration.
        /// </summary>
        public List<String> CacheNames { get; }

        ServiceConfiguration Build(
            Dictionary<Type, TypeFactory> typeFactories,
            IEnumerable<CustomAction<ServiceConfiguration, ServiceConfigurationBuilder>> allSetups);
    }
}
