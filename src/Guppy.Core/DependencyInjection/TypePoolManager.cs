using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection
{
    /// <summary>
    /// A collection of ServicePool instances
    /// sorted by type.
    /// </summary>
    public class TypePoolManager
    {
        #region Private Fields
        private UInt16 _maxPoolSize;
        private Dictionary<Type, TypePool> _pools;
        private TypePool _pool;
        #endregion

        #region Public Properties
        public UInt16 MaxPoolSize
        {
            get => _maxPoolSize;
            set => _maxPoolSize = value;
        }

        public TypePool this[Type type]
        {
            get
            {
                if (!_pools.TryGetValue(type, out _pool))
                {
                    _pool = new TypePool(type, ref _maxPoolSize);
                    _pools.Add(type, _pool);
                }

                return _pool;
            }
        }

        public TypePool this[Object type] => this[type.GetType()];
        #endregion

        #region Constructors
        public TypePoolManager(ref UInt16 maxPoolSize)
        {
            _pools = new Dictionary<Type, TypePool>();
            _maxPoolSize = maxPoolSize;
        }
        #endregion
    }
}
