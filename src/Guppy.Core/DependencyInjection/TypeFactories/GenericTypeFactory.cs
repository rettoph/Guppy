using Guppy.DependencyInjection.Actions;
using Guppy.DependencyInjection.Dtos;
using Guppy.DependencyInjection.TypePools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guppy.Extensions.System;

namespace Guppy.DependencyInjection.TypeFactories
{
    internal class GenericTypeFactory : ITypeFactory
    {
        #region Private Fields
        private TypeFactoryDto _context;
        private GenericTypePool _pool;
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
        internal GenericTypeFactory(
            TypeFactoryDto context,
            IEnumerable<BuilderAction> builders)
        {
            _context = context;
            _maxPoolSize = context.MaxPoolSize;
            _pool = new GenericTypePool(_context.ImplementationType, ref _maxPoolSize);

            this.BuilderActions = builders
                .Where(b => b.Filter(this.Type))
                .OrderBy(b => b.Order)
                .ToArray();

            this.MaxPoolSize = _context.MaxPoolSize;
        }
        #endregion

        /// <inheritdoc />
        public object BuildInstance(GuppyServiceProvider provider, Type[] generics)
        {
            var type = this.ImplementationType.MakeGenericType(generics);

            if (!_pool.TryPull(type, out Object instance))
            {
                instance = _context.Method(provider, type);

                foreach (BuilderAction action in this.BuilderActions)
                    action.Invoke(instance, provider, this);
            }

            return instance;
        }

        /// <inheritdoc />
        public Boolean TryReturnToPool(Object instance)
            => _pool.TryReturn(instance);

        /// <inheritdoc />
        public IEnumerable<ServiceConfigurationKey> MutateKeys(ServiceConfigurationKey[] keys, Type[] generics)
        {
            foreach (ServiceConfigurationKey key in keys)
                yield return key.TryConstructGenericKey(generics);
        }

        /// <inheritdoc />
        public ServiceConfigurationKey MutateKey(ServiceConfigurationKey key, Type[] generics)
            => key.TryConstructGenericKey(generics);
    }
}
