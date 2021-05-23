using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection.Descriptors
{
    public class ServiceConfigurationDescriptor
    {
        #region Public Fields
        /// <summary>
        /// A unique key for the current service
        /// configuration.
        /// </summary>
        public readonly ServiceConfigurationKey Key;

        /// <summary>
        /// The default lifetime for the described service.
        /// </summary>
        public readonly ServiceLifetime Lifetime;

        /// <summary>
        /// The unlinked factory type used to build a fresh
        /// instance as needed.
        /// </summary>
        public readonly Type TypeFactory;

        /// <summary>
        /// <para>
        /// The base type with which the described service will be cached
        /// when <see cref="Lifetime"/> is <see cref="ServiceLifetime.Scoped"/>
        /// or <see cref="ServiceLifetime.Singleton"/>. By default this will
        /// be set to the <see cref="TypeFactory"/> type.
        /// </para>
        /// <para>
        /// Additionally, all <see cref="Type"/>s between 
        /// the <see cref="BaseLookupType"/> and
        /// <see cref="ServiceConfigurationKey.Type"/> 
        /// wil be cached as well.
        /// </para>
        /// </summary>
        public readonly Type BaseLookupType;

        /// <summary>
        /// The priority value for this specific descriptor.
        /// Only the highest priority for each index will be 
        /// saved and used within the ServiceProvider.
        /// </summary>
        public readonly Int32 Priority;
        #endregion

        #region Constructor
        public ServiceConfigurationDescriptor(
            ServiceConfigurationKey key, 
            ServiceLifetime lifetime,
            Type typeFactory = default,
            Type baseLookupType = default,
            int priority = 0)
        {
            this.Key = key;
            this.Lifetime = lifetime;
            this.TypeFactory = typeFactory ?? key.Type;
            this.BaseLookupType = baseLookupType ?? this.TypeFactory;
            this.Priority = priority;
        }
        #endregion
    }
}
