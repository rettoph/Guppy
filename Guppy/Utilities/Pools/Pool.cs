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

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            this.logger.LogDebug($"Creating new Pool<{this.GetType().Name}>({this.Id}) for Type<{this.TargetType.Name}>.");
        }
        #endregion

        #region Abstract Methods
        protected abstract Object Build(IServiceProvider provider);
        #endregion

        #region Helper Methods
        public virtual void Put(Object instance)
        {
            _pool.Enqueue(instance);
        }

        public virtual Object Pull(Action<Object> setup = null)
        {
            return this.Pull<Object>(setup);
        }
        public virtual T Pull<T>(Action<T> setup = null)
            where T : class
        {
            ExceptionHelper.ValidateAssignableFrom<T>(this.TargetType);

            T child;

            if (_pool.Count == 0)
            {
                this.logger.LogDebug($"Pool<{this.GetType().Name}>({this.Id}) => Building new Type<{this.TargetType.Name}> instance.");
                child = this.Build(this.provider) as T;
            }

            else
            {
                child = _pool.Dequeue() as T;
                this.logger.LogDebug($"Pool<{this.GetType().Name}>({this.Id}) => Reusing old Type<{this.TargetType.Name}> instance.");
            }

            // Run the custom setup method if any
            setup?.Invoke(child);

            return child;
        }
        #endregion
    }
}
