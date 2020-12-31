using Guppy.DependencyInjection.Descriptors;
using Guppy.Extensions.System.Collections;
using Guppy.Extensions.System;
using Guppy.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guppy.Extensions.log4net;

namespace Guppy.DependencyInjection
{
    public sealed class ServiceFactory : ServiceFactoryDescriptor
    {
        #region Private Fields
        private ServicePool _pool;
        private UInt32 _created;
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
            Type type = this.Type;

            if (generics != default && generics.Any())
                type = cacheType = this.Type.MakeGenericType(generics);


            _pool = this.Pools[type];
            if (_pool.Any())
                return _pool.Pull(cacher);

            _created++;
#if DEBUG_VERBOSE
            provider.logger?.Verbose($"Created => {type.GetPrettyName()} ({_created})");
#endif
            return this.Factory(provider, type).Then(i =>
            {
                cacher?.Invoke(cacheType, i);
                foreach (ServiceAction action in configuration?.Actions[ServiceActionType.Builder])
                    action.Excecute(i, provider, configuration);
            });
        }
        #endregion
    }
}
