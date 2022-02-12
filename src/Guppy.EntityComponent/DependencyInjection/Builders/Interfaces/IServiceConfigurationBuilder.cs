using Minnow.General.Interfaces;
using System;
using System.Collections.Generic;

namespace Guppy.EntityComponent.DependencyInjection.Builders.Interfaces
{
    public interface IServiceConfigurationBuilder : IPrioritizable
    {
        /// <summary>
        /// The primary lookup type bound to this service.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// The lookup key of the service's <see cref="TypeFactory"/>.
        /// If undefined this will be defaulted to the current <see cref="Type"/>
        /// value.
        /// </summary>
        public Type FactoryType { get; }

        /// <summary>
        /// The service lifetime.
        /// </summary>
        public ServiceLifetime Lifetime { get; }

        /// <summary>
        /// A list alternative lookup types that may be linked to this service.
        /// The current <see cref="Type"/> must be assignable to all of the
        /// given aliases.
        /// </summary>
        public HashSet<Type> Aliases { get; }

        ServiceConfiguration Build(
            Dictionary<Type, TypeFactory> typeFactories,
            IEnumerable<CustomAction<ServiceConfiguration, IServiceConfigurationBuilder>> allSetups);
    }
}
