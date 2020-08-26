using Guppy.DependencyInjection.Enums;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.DependencyInjection
{
    public sealed class ServiceFactory
    {
        #region Private Fields
        /// <summary>
        /// The internal factory method, only used when no items pooled.
        /// </summary>
        private Func<ServiceProvider, Object> _factory;

        /// <summary>
        /// A small collection of pooled item instances to be
        /// returned over a new instance.
        /// </summary>
        private Stack<Object> _pool;

        /// <summary>
        /// The number of items contained within the pool.
        /// </summary>
        private Int32 _count;
        #endregion

        #region Public Fields
        /// <summary>
        /// The base type returned by this factory instance.
        /// </summary>
        public readonly Type Type;
        #endregion

        #region Constructor
        internal ServiceFactory(ServiceFactoryData data)
        {
            _count = 0;
            _pool = new Stack<Object>(GuppySettings.MaxServicePoolSize);
            _factory = data.Factory;

            this.Type = data.Type;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Create or reuse a new service instance.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public Object Build(ServiceProvider provider)
        {
            if (_count != 0)
            { // If an instance is in the pool...
                _count--;
                return _pool.Pop();
            }
                

            // Build a new instance...
            return _factory.Invoke(provider);
        }

        /// <summary>
        /// Return a service instance into the internal pool.
        /// </summary>
        /// <param name="instance"></param>
        public void Return(Object instance)
        {
            ExceptionHelper.ValidateAssignableFrom(this.Type, instance.GetType());

            if(_count < GuppySettings.MaxServicePoolSize)
            { // If the pool has smace remaining...
                _count++;
                _pool.Push(instance);
            }
        }
        #endregion
    }
}
