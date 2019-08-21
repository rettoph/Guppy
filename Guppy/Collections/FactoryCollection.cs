using Guppy.Implementations;
using Guppy.Utilities;
using Guppy.Utilities.Factories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Collections
{
    /// <summary>
    /// Represents a collection of items
    /// that can be created via the Guppy
    /// pooling service.
    /// 
    /// Any types used within must first be
    /// registered via the PoolLoader
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FactoryCollection<T> : FrameableCollection<T>
        where T : Frameable
    {
        #region Private Fields
        private PooledFactory<T> _factory;
        #endregion

        #region Constructor
        public FactoryCollection(PooledFactory<T> factory, IServiceProvider provider) : base(provider)
        {
            _factory = factory;

            this.Events.Register<T>("created");
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Create a new instance and add it to the collection
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public T Create(Type type, Action<T> setup = null)
        {
            return this.Create<T>(type, setup);
        }
        /// <summary>
        /// Create a new instance and add it to the collection
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="setup"></param>
        /// <returns></returns>
        public TOut Create<TOut>(Action<TOut> setup = null)
            where TOut : T
        {
            return this.Create<TOut>(typeof(TOut), setup);
        }
        /// <summary>
        /// Create a new instance and add it to the collection
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="type"></param>
        /// <param name="setup"></param>
        /// <returns></returns>
        public TOut Create<TOut>(Type type, Action<TOut> setup = null)
            where TOut : T
        {
            ExceptionHelper.ValidateAssignableFrom<TOut>(type);

            // Create a new scene instance
            var instance = _factory.Pull<TOut>(setup);
            this.Events.Invoke<T>("created", this, instance);

            // Add the scene instance to the collection
            this.Add(instance);

            return instance;
        }
        #endregion
    }
}
