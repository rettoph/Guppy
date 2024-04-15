using Guppy.Core.Common;
using Guppy.Core.Serialization.Common.Factories;
using Guppy.Core.Serialization.Common.Services;
using System.Runtime.InteropServices;

namespace Guppy.Core.Serialization.Services
{
    internal sealed class DefaultInstanceService : IDefaultInstanceService
    {
        private IDefaultInstanceFactory[] _factories;
        private Dictionary<Type, IDefaultInstanceFactory> _typeFactories;

        public DefaultInstanceService(IFiltered<IDefaultInstanceFactory> factories)
        {
            _factories = factories.Instances.ToArray();
            _typeFactories = new Dictionary<Type, IDefaultInstanceFactory>();
        }

        public T Get<T>()
        {
            return (T)Get(typeof(T))!;
        }

        public object Get(Type type)
        {
            return this.GetFactory(type).BuildInstance(type);
        }

        private IDefaultInstanceFactory GetFactory(Type type)
        {
            ref IDefaultInstanceFactory? typeFactory = ref CollectionsMarshal.GetValueRefOrAddDefault(_typeFactories, type, out bool exists);
            if (exists)
            {
                return typeFactory!;
            }

            typeFactory = DefaultActivatorFactory.Instance;
            foreach (var factory in _factories)
            {
                if (factory.CanConstructType(type))
                {
                    typeFactory = factory;
                    break;
                }
            }

            return typeFactory;
        }

        private class DefaultActivatorFactory : IDefaultInstanceFactory
        {
            public static DefaultActivatorFactory Instance = new DefaultActivatorFactory();

            public bool CanConstructType(Type type)
            {
                return true;
            }

            public object BuildInstance(Type type)
            {
                if (type.IsInterface)
                {
                    throw new ArgumentException();
                }

                if (type.IsAbstract)
                {
                    throw new ArgumentException();
                }

                if (type.IsGenericTypeDefinition)
                {
                    throw new ArgumentException();
                }

                return Activator.CreateInstance(type) ?? throw new NotImplementedException();
            }
        }
    }
}
