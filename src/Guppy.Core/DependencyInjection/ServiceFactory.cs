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
        private Stack<Object> _pool;
        private UInt16 _poolSize;
        private Func<ServiceProvider, Object> _factory;
        #endregion

        #region Public Properties
        public UInt16 MaxPoolSize { get; set; }
        #endregion

        #region Constructors
        public ServiceFactory(
            ServiceFactoryDescriptor descriptor) : base(descriptor.Type, descriptor.Factory, descriptor.ImplementationType)
        {
            _pool = new Stack<Object>();
            _poolSize = 0;
            this.MaxPoolSize = 25;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Build a new service instance (or pull once from the pool)
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="cacher">A simple method to run before returning the instance. Useful for caching the scope or singleton values.</param>
        /// <param name="configuration">The calling configuration instance.</param>
        /// <returns></returns>
        public Object Build(ServiceProvider provider, Action<Type, Object> cacher = null, ServiceConfiguration configuration = null)
        {
            if (_pool.Any())
                return _pool.Pop().Then(i => cacher?.Invoke(this.Type, cacher));

            return _factory(provider).Then(i =>
            {
                cacher?.Invoke(this.Type, cacher);
                configuration?.Actions[ServiceActionType.Builder].ForEach(b => b.Excecute(i, provider, configuration));
            });
        }

        /// <summary>
        /// If the internal pool is not yet at max capacity,
        /// return the recieved instance into the pool.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public Boolean TryReturn(Object instance)
        {
            ExceptionHelper.ValidateAssignableFrom(this.ImplementationType, instance.GetType());

            if(_poolSize < this.MaxPoolSize)
            {
                _pool.Push(instance);
                _poolSize++;
                return true;
            }

            return false;
        }
        #endregion
    }
}
