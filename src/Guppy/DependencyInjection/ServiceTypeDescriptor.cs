using Guppy.Extensions.Collections;
using Guppy.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.DependencyInjection
{
    public class ServiceTypeDescriptor
    {
        #region Private Fields
        private Queue<Object> _pool;
        private Int32 _poolCount;
        private Object _instance;
        #endregion

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
        /// <summary>
        /// The maximum number of instances to pool within the internal service pool.
        /// Default: 50
        /// </summary>
        public Int32 MaxPoolSize { get; set; }
        #endregion

        #region Constructor
        public ServiceTypeDescriptor()
        {
            _pool = new Queue<Object>();

            this.MaxPoolSize = 50;
        }
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
                    Object instance = null;
                    configuration.Build(provider, setup, i => i = instance = i);
                    return instance;
                case ServiceLifetime.Scoped:
                    if (!provider.scopedInstances.ContainsKey(this.CacheType))
                        configuration.Build(provider, setup, i => provider.scopedInstances[this.CacheType] = i);
                    return provider.scopedInstances[this.CacheType];
                case ServiceLifetime.Singleton:
                    if (!provider.singletonInstances.ContainsKey(this.CacheType))
                        configuration.Build(provider, setup, i => provider.singletonInstances[this.CacheType] = i);
                    return provider.singletonInstances[this.CacheType];
                default:
                    throw new Exception($"Unable to create instance, unknown service lifetime value ({this.Lifetime}).");
            }
        }

        /// <summary>
        /// Release the item and return it back into its pool.
        /// pooled items will be automatically returned 
        /// instead of calling the Factory method.
        /// </summary>
        /// <param name="instance"></param>
        public void Release(Object instance)
        {
            ExceptionHelper.ValidateAssignableFrom(this.ServiceType, instance.GetType());

            if(_poolCount < this.MaxPoolSize)
            {
                _pool.Enqueue(instance);
                _poolCount++;
            }
        }

        /// <summary>
        /// Return a raw & unconfigured instance of the 
        /// current service. This will either pull from
        /// the pool or build a new instance as needed.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public Object Get(ServiceProvider provider)
        {
            if (_pool.Any())
            {
                _instance = _pool.Dequeue();
                _poolCount--;
            }
            else
                _instance = this.Factory(provider);

            return _instance;
        }
        #endregion
    }
}
