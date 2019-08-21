using Guppy.Implementations;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities.Pools
{
    /// <summary>
    /// Used to generate or reuse instances of an object.
    /// </summary>
    public abstract class Pool : Creatable
    {
        #region Private Fields
        private Queue<Object> _pool;
        #endregion

        #region Public Fields
        public Type TargetType;
        #endregion

        #region Constructor
        public Pool(Type targetType)
        {
            _pool = new Queue<Object>();
            this.TargetType = targetType;
        }
        #endregion

        #region Abstract Methods
        protected abstract Object Create(IServiceProvider provider);
        #endregion

        #region Helper Methods
        public virtual void Put(Object instance)
        {
            _pool.Enqueue(instance);
        }

        public virtual Object Pull(IServiceProvider provider, Action<Object> setup = null)
        {
            return this.Pull<Object>(provider, setup);
        }
        public virtual T Pull<T>(IServiceProvider provider, Action<T> setup = null)
            where T : class
        {
            T child;

            if (_pool.Count == 0)
            {
                this.logger.LogDebug($"Pooling: Creating new {this.TargetType.Name} instance.");
                child = this.Create(provider) as T;
            }

            else
            {
                this.logger.LogDebug($"Pooling: Reusing old {this.TargetType.Name} instance.");
                child = _pool.Dequeue() as T;
            }

            // Run the custom setup method if any
            setup?.Invoke(child);

            return child;
        }
        #endregion
    }
}
