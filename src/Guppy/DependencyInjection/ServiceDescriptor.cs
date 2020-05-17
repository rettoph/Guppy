using Guppy.Extensions.Collections;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection
{
    public class ServiceDescriptor
    {
        #region Public Attributes
        /// <summary>
        /// Optional, only used when Lifetime is scope or singleton. This defines
        /// how a scoped instance is saved, allowing for custom scoped 
        /// services that share the same base type (seen primarily within
        /// scenes).
        /// </summary>
        public Type CacheType { get; set; }
        /// <summary>
        /// The service type itself. This defines the main key at which the
        /// current service is returned.
        /// </summary>
        public Type ServiceType { get; set; }
        public ServiceLifetime Lifetime { get; set; }
        public Func<ServiceProvider, Object> Factory { get; set; }
        public Int32 Priority { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Returns an instance of the current service utilizing the recieved
        /// ConfigurationDescriptor array if a new instance is created. 
        /// If a cached service instance is returned, the recieved 
        /// ConfigurationDescriptor  arraywill simply be ignored.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public Object GetInstance(ServiceProvider provider, ServiceConfiguration configuration, Action<Object, ServiceProvider, ServiceConfiguration> setup)
        {
            switch (this.Lifetime)
            {
                case ServiceLifetime.Transient:
                    return configuration.Build(provider, setup);
                case ServiceLifetime.Scoped:
                    if (!provider.scopedInstances.ContainsKey(this.CacheType))
                        provider.scopedInstances[this.CacheType] = configuration.Build(provider, setup);
                    return provider.scopedInstances[this.CacheType];
                case ServiceLifetime.Singleton:
                    if (!provider.singletonInstances.ContainsKey(this.CacheType))
                        provider.singletonInstances[this.CacheType] = configuration.Build(provider, setup);
                    return provider.singletonInstances[this.CacheType];
                default:
                    throw new Exception($"Unable to create instance, unknown service lifetime value ({this.Lifetime}).");
            }
        }
        #endregion
    }
}
