using Guppy.Pooling.Interfaces;
using Guppy.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Pooling
{
    /// <summary>
    /// A Pool is a creatable object that can
    /// build multiple instances of a specific
    /// type.
    /// 
    /// Instances can be returned to the pool
    /// at any time, after which they will be
    /// reused the next time an instance is
    /// requested.
    /// </summary>
    internal sealed class Pool : IPool
    {
        #region Private Fields
        /// <summary>
        /// A queue of available instances the pool can return instead of building new ones.
        /// </summary>
        private Queue<Object> _available;

        private ILogger _logger;
        #endregion

        #region Public Attributes
        /// <summary>
        /// The type this pool will currently make.
        /// </summary>
        public Type TargetType { get; private set; }
        #endregion

        #region Constructor
        public Pool(Type targetType, ILogger logger)
        {
            _available = new Queue<Object>();
            _logger = logger;

            this.TargetType = targetType;
        }
        #endregion

        #region Helper Methods
        public Object Pull(Func<Type, Object> factory)
        {
            if (_available.Count == 0)
            {
                _logger.LogTrace($"Pool<{this.TargetType.Name}>({_available.Count}) => Creating new instance...");
                return factory(this.TargetType);
            }
            else
            {
                _logger.LogTrace($"Pool<{this.TargetType.Name}>({_available.Count}) => Pulling old instance from pool...");
                return _available.Dequeue();
            }
        }

        public void Put(Object instance)
        {
            ExceptionHelper.ValidateAssignableFrom(this.TargetType, instance.GetType());

            _logger.LogTrace($"Pool<{this.TargetType.Name}>({_available.Count}) => Returning old instance to pool...");

            _available.Enqueue(instance);
        }
        #endregion
    }
}
