using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection
{
    /// <summary>
    /// A collection of ServicePool instances
    /// sorted by type.
    /// </summary>
    public class ServicePoolManager
    {
        #region Private Fields
        private UInt16 _maxPoolSize;
        private Dictionary<Type, ServicePool> _pools;
        private ServicePool _pool;
        #endregion

        #region Public Properties
        public UInt16 MaxPoolSize
        {
            get => _maxPoolSize;
            set => _maxPoolSize = value;
        }

        public ServicePool this[Type type]
        {
            get
            {
                if (!_pools.TryGetValue(type, out _pool))
                {
                    _pool = new ServicePool(type, ref _maxPoolSize);
                    _pools.Add(type, _pool);
                }

                return _pool;
            }
        }

        public ServicePool this[Object type] => this[type.GetType()];
        #endregion

        #region Constructors
        public ServicePoolManager()
        {
            _pools = new Dictionary<Type, ServicePool>();
            _maxPoolSize = 25;
        }
        #endregion
    }
}
