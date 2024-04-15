using Guppy.Core.Common.Services;
using System.Collections;

namespace Guppy.Core.Services
{
    internal class TypeService<T> : ITypeService<T>
    {
        public readonly List<Type> _types;

        public TypeService(IEnumerable<Type> types)
        {
            _types = new List<Type>(types);
        }

        public IEnumerable<T> CreateInstances()
        {
            foreach (Type type in _types)
            {
                object? instance = Activator.CreateInstance(type);

                if (instance is not null && instance is T casted)
                {
                    yield return casted;
                }

                throw new InvalidOperationException();
            }
        }

        public IEnumerator<Type> GetEnumerator()
        {
            return _types.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
