using Guppy.Core.Common;
using Guppy.Core.Serialization.Common.Factories;
using Guppy.Core.Serialization.Common.Services;
using System.Runtime.InteropServices;

namespace Guppy.Core.Serialization.Services
{
    internal sealed class DefaultInstanceService(IFiltered<IDefaultInstanceFactory> factories) : IDefaultInstanceService
    {
        private readonly IFiltered<IDefaultInstanceFactory> _factories = factories;
        private readonly Dictionary<Type, IDefaultInstanceFactory> _typeFactories = [];

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
            public static DefaultActivatorFactory Instance = new();

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
