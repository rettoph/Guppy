using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Services
{
    public interface ICollectionService<TId, T> : IEnumerable<T>
        where T : class
    {
        T this[TId id] { get; }

        bool TryGetById(TId id, [MaybeNullWhen(false)] out T item);

        bool Contains(T item);
    }
}
