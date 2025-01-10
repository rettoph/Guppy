using System.Collections;
using Guppy.Core.Common.Services;

namespace Guppy.Core.Services
{
    internal class TypeService<T>(IEnumerable<Type> types) : ITypeService<T>
    {
        private readonly List<Type> _types = new(types);

        public IEnumerable<T> CreateInstances()
        {
            foreach (Type type in this._types)
            {
                object? instance = Activator.CreateInstance(type);

                if (instance is not null and T casted)
                {
                    yield return casted;
                }

                throw new InvalidOperationException();
            }
        }

        public IEnumerator<Type> GetEnumerator()
        {
            return this._types.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}