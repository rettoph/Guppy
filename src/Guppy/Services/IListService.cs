using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Services
{
    public interface IListService<TKey, T> : ICollectionService<TKey, T>
        where T : class
    {
        event OnEventDelegate<IListService<TKey, T>, T>? OnItemAdded;
        event OnEventDelegate<IListService<TKey, T>, T>? OnItemRemoved;

        bool TryAdd(T item);
        bool TryRemove(T item);
        bool TryCreate<TItem>([MaybeNullWhen(false)] out TItem item)
            where TItem : class, T;
        TItem Create<TItem>()
            where TItem : class, T;

        bool TryCreate(Type type, [MaybeNullWhen(false)] out T item);
        T Create(Type type);
    }
}
