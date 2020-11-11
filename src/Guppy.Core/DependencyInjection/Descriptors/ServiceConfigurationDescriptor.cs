using Guppy.Extensions.System;
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
        /// A unique index for the current service
        /// configuration.
        /// </summary>
        public readonly UInt32 Id;

        /// <summary>
        /// A unique name for the current service
        /// configuration. This should be human readable
        /// and will be defined as the FactoryType.FullName
        /// by default.
        /// </summary>
        public readonly String Name;

        /// <summary>
        /// The default lifetime for the described service.
        /// </summary>
        public readonly ServiceLifetime Lifetime;

        /// <summary>
        /// The unlinked factory type used to build a fresh
        /// instance as needed.
        /// </summary>
        public readonly Type Factory;

        /// <summary>
        /// The priority value for this specific descriptor.
        /// Only the highest priority for each index will be 
        /// saved and used within the ServiceProvider.
        /// </summary>
        public readonly Int32 Priority;
        #endregion

        #region Constructor
        public ServiceConfigurationDescriptor(string name, ServiceLifetime lifetime, Type factory, int priority = 0)
        {
            this.Id = name.xxHash();
            this.Name = name;
            this.Lifetime = lifetime;
            this.Factory = factory;
            this.Priority = priority;
        }
        #endregion

        #region Static Helper Methods
        public static ServiceConfigurationDescriptor Singleton(String name, Type factory, int priority = 0)
            => new ServiceConfigurationDescriptor(name, ServiceLifetime.Singleton, factory, priority);
        public static ServiceConfigurationDescriptor Singleton<TFactory>(String name, int priority = 0)
            => new ServiceConfigurationDescriptor(name, ServiceLifetime.Singleton, typeof(TFactory), priority);

        public static ServiceConfigurationDescriptor Singleton<TFactory>(int priority = 0)
            => new ServiceConfigurationDescriptor(typeof(TFactory).FullName, ServiceLifetime.Singleton, typeof(TFactory), priority);

        public static ServiceConfigurationDescriptor Scoped(String name, Type factory, int priority = 0)
            => new ServiceConfigurationDescriptor(name, ServiceLifetime.Scoped, factory, priority);
        public static ServiceConfigurationDescriptor Scoped<TFactory>(String name, int priority = 0)
            => new ServiceConfigurationDescriptor(name, ServiceLifetime.Scoped, typeof(TFactory), priority);

        public static ServiceConfigurationDescriptor Scoped<TFactory>(int priority = 0)
            => new ServiceConfigurationDescriptor(typeof(TFactory).FullName, ServiceLifetime.Scoped, typeof(TFactory), priority);

        public static ServiceConfigurationDescriptor Transient(String name, Type factory, int priority = 0)
            => new ServiceConfigurationDescriptor(name, ServiceLifetime.Transient, factory, priority);
        public static ServiceConfigurationDescriptor Transient<TFactory>(String name, int priority = 0)
            => new ServiceConfigurationDescriptor(name, ServiceLifetime.Transient, typeof(TFactory), priority);

        public static ServiceConfigurationDescriptor Transient<TFactory>( int priority = 0)
            => new ServiceConfigurationDescriptor(typeof(TFactory).FullName, ServiceLifetime.Transient, typeof(TFactory), priority);
        #endregion
    }
}
