using Guppy.DependencyInjection.Descriptors;
using Guppy.Extensions.Collections;
using Guppy.Extensions.System;
using Guppy.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.DependencyInjection
{
    public sealed class ServiceFactory : ServiceFactoryDescriptor
    {
        #region Private Fields
        private ServicePool _pool;
        #endregion

        #region Public Properties
        public ServicePoolManager Pools { get; set; }
        #endregion

        #region Constructors
        public ServiceFactory(
            ServiceFactoryDescriptor descriptor) : base(descriptor.Type, descriptor.Factory, descriptor.ImplementationType)
        {
            this.Pools = new ServicePoolManager();
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Build a new service instance (or pull once from the pool)
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="cacheType">The type to be passed into the <paramref name="cacher"/> if any.</param>
        /// <param name="cacher">A simple method to run before returning the instance. Useful for caching the scope or singleton values.</param>
        /// <param name="configuration">The calling configuration instance.</param>
        /// <param name="generics">The specific type to construct (generally used for generic types).</param>
        /// <returns></returns>
        public Object Build(ServiceProvider provider, Type cacheType, Action<Type, Object> cacher = null, ServiceConfiguration configuration = null, Type[] generics = null)
        {
            if (generics != default && generics.Any())
            {
                _pool = this.Pools[this.Type.MakeGenericType(generics)];
                cacheType = cacheType.MakeGenericType(generics);
            }
            else
                _pool = this.Pools[this.Type];

            if (_pool.Any())
                return _pool.Pull(cacher);

            return this.Factory(provider, this.Type).Then(i =>
            {
                cacher?.Invoke(cacheType, i);
                configuration?.Actions[ServiceActionType.Builder].ForEach(b => b.Excecute(i, provider, configuration));
            });
        }
        #endregion
    }
}
