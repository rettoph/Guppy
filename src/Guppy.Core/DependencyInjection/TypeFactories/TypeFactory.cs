using Guppy.DependencyInjection.Actions;
using Guppy.DependencyInjection.Contexts;
using Guppy.DependencyInjection.ServiceConfigurations;
using Guppy.DependencyInjection.TypePools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.DependencyInjection.TypeFactories
{
    internal class TypeFactory : ITypeFactory
    {
        #region Private Fields
        private TypeFactoryContext _context;
        private TypePool _pool;
        private UInt16 _maxPoolSize;
        #endregion

        #region Public Fields
        /// <inheritdoc />
        public Type Type => _context.Type;

        /// <inheritdoc />
        public Type ImplementationType => _context.ImplementationType;

        /// <inheritdoc />
        public BuilderAction[] BuilderActions { get; private set; }

        /// <inheritdoc />
        public UInt16 MaxPoolSize
        {
            get => _maxPoolSize;
            set => _maxPoolSize = value;
        }
        #endregion

        #region Constructor
        internal TypeFactory(
            TypeFactoryContext context,
            IEnumerable<BuilderAction> builders)
        {
            _context = context;
            _maxPoolSize = context.MaxPoolSize;
            _pool = new TypePool(_context.ImplementationType, ref _maxPoolSize);

            this.MaxPoolSize = _context.MaxPoolSize;

            this.BuilderActions = builders.Where(b =>
            {
                return b.Key.IsAssignableFrom(this.Type)
                    || this.Type.IsSubclassOf(b.Key);
            }).ToArray();
        }
        #endregion

        /// <inheritdoc />
        public object BuildInstance(ServiceProvider provider, Type[] generics)
            => _context.Method(provider, this.ImplementationType);

        /// <inheritdoc />
        public void TryReturnToPool(Object instance)
            => _pool.TryReturn(instance);

        /// <inheritdoc />
        public IEnumerable<ServiceConfigurationKey> MutateKeys(ServiceConfigurationKey[] keys, Type[] generics)
            => keys;

        /// <inheritdoc />
        public ServiceConfigurationKey MutateKey(ServiceConfigurationKey key, Type[] generics)
            => key;
    }
}
