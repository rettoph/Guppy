using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Services
{
    public interface ICollectionService<TKey, T> : IEnumerable<T>
        where T : class
    {
        T this[TKey key] { get; }

        bool TryGet(TKey key, [MaybeNullWhen(false)] out T item);
        T Get(TKey key);

        bool Contains(T item);
    }
}
