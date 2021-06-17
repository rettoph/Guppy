using Guppy.DependencyInjection.Actions;
using Guppy.DependencyInjection.Contexts;
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
        private TypeFactoryContext _context;
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
            TypeFactoryContext context,
            IEnumerable<BuilderAction> builders)
        {
            _context = context;
            _maxPoolSize = context.MaxPoolSize;
            _pool = new GenericTypePool(_context.ImplementationType, ref _maxPoolSize);

            this.BuilderActions = builders.Where(b =>
            {
                return b.Key.IsAssignableFrom(this.Type)
                    || this.Type.IsSubclassOf(b.Key)
                    || this.Type.IsSubclassOfRawGeneric(b.Key);
            }).ToArray();

            this.MaxPoolSize = _context.MaxPoolSize;
        }
        #endregion

        /// <inheritdoc />
        public object BuildInstance(ServiceProvider provider, Type[] generics)
        {
            var instance = _context.Method(provider, this.ImplementationType.MakeGenericType(generics));

            foreach (BuilderAction action in this.BuilderActions)
                action.Invoke(instance, provider, this);

            return instance;
        }

        /// <inheritdoc />
        public void TryReturnToPool(Object instance)
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
