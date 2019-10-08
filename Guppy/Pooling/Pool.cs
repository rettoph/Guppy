using Guppy.Pooling.Interfaces;
using Guppy.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
        private ConcurrentQueue<Object> _available;

        private ILogger _logger;

        private Object _dequeued;
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
            _available = new ConcurrentQueue<Object>();
            _logger = logger;

            this.TargetType = targetType;
        }
        #endregion

        #region Helper Methods
        public Object Pull(Func<Type, Object> factory)
        {
            if(!_available.TryDequeue(out _dequeued))
                return factory(this.TargetType);

            return _dequeued;
        }

        public void Put(Object instance)
        {
            ExceptionHelper.ValidateAssignableFrom(this.TargetType, instance.GetType());

            _available.Enqueue(instance);
        }

        public int Count()
        {
            return _available.Count();
        }
        #endregion
    }
}
