using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Services
{
    public interface IListService<TId, T> : ICollectionService<TId, T>
        where T : class
    {
        event OnEventDelegate<IListService<TId, T>, T>? OnItemAdded;
        event OnEventDelegate<IListService<TId, T>, T>? OnItemRemoved;

        bool TryAdd(T item);
        bool TryRemove(T item);
        bool TryCreate<TItem>(out TItem? item)
            where TItem : class, T;
    }
}
