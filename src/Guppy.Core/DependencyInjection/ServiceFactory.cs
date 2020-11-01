using Guppy.DependencyInjection;
using Guppy.DependencyInjection.Structs;
using Guppy.Extensions.Collections;
using Guppy.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.DependencyInjection
{
    public sealed class ServiceFactory
    {
        #region Static Fields
        public static Int32 MaxServicePoolSize { get; set; } = 512;
        #endregion

        #region Private Fields
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
        /// The builders to be excecuted when constructing a new instance.
        /// </summary>
        public readonly ServiceBuilder[] Builders;

        /// <summary>
        /// The primary Microsoft service descriptor linked 
        /// to this factory.
        /// </summary>
        public readonly ServiceDescriptor Descriptor;
        #endregion

        #region Public Properties
        /// <summary>
        /// The primary factory type. This is either the descriptor implementation type
        /// or the descripto service type.
        /// </summary>
        public Type Type => this.Descriptor.ImplementationType ?? this.Descriptor.ServiceType;
        #endregion

        #region Constructor
        internal ServiceFactory(ServiceFactoryData data, ServiceBuilder[] builders)
        {
            _count = 0;
            _pool = new Stack<Object>(ServiceFactory.MaxServicePoolSize);

            this.Descriptor = data.Descriptor;
            this.Builders = builders;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Create or reuse a new service instance.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public Object Build(GuppyServiceProvider provider)
        {
            if (_count != 0)
            { // If an instance is in the pool...
                _count--;
                return _pool.Pop();
            }

            // Build a new instance...
            Object instance;
            if(this.Descriptor.ImplementationFactory == default)
            {
                instance = ActivatorUtilities.CreateInstance(provider, this.Type);
            }
            else
            {
                instance = this.Descriptor.ImplementationFactory.Invoke(provider);
            }

            this.Builders.ForEach(b => b.Build(instance, provider));

            return instance;
        }

        /// <summary>
        /// Return a service instance into the internal pool.
        /// </summary>
        /// <param name="instance"></param>
        public void Return(Object instance)
        {
            ExceptionHelper.ValidateAssignableFrom(this.Descriptor.ImplementationType, instance.GetType());

            if(_count < ServiceFactory.MaxServicePoolSize)
            { // If the pool has smace remaining...
                _count++;
                _pool.Push(instance);
            }
        }
        #endregion
    }
}
