using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection
{
    /// <summary>
    /// The primary configuration class for customizing
    /// service instances.
    /// </summary>
    public sealed class ServiceConfiguration
    {
        #region Public Fields
        /// <summary>
        /// The configurations unique index, used to define which services
        /// this configuration should be applied to.
        /// </summary>
        public readonly ServiceConfigurationKey Key;

        /// <summary>
        /// The configuration method's numerical order. Defines
        /// when to execute the configuration method when pulling
        /// a new service instance.
        /// </summary>
        public readonly Int32 Order;
        #endregion

        #region Private Fields
        private readonly Action<Object, GuppyServiceProvider, ServiceContext> _configuration;
        #endregion

        #region Constructor
        internal ServiceConfiguration(
            ServiceConfigurationKey key, 
            Action<Object, GuppyServiceProvider, ServiceContext> configuration,
            Int32 Order)
        {
            _configuration = configuration;

            this.Key = key;
            this.Order = Order;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Execute the configuration method on the recieved instance.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="provider"></param>
        /// <param name="descriptor"></param>
        public void Configure(Object instance, GuppyServiceProvider provider, ServiceContext descriptor)
            => _configuration.Invoke(instance, provider, descriptor);
        #endregion
    }
}
