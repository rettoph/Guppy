using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Services.Common
{
    public abstract class CollectionService<TId, T> : ICollectionService<TId, T>
        where TId : notnull
        where T : class
    {
        protected Dictionary<TId, T> items;

        public virtual T this[TId id] => this.items[id];

        public CollectionService(int capacity = 0)
        {
            this.items = new Dictionary<TId, T>(capacity);
        }
        public CollectionService(IEnumerable<T> items)
        {
            this.items = new Dictionary<TId, T>(items.Select(x => new KeyValuePair<TId, T>(this.GetId(x), x)));
        }

        protected abstract TId GetId(T item);

        public bool TryGetById(TId id, [MaybeNullWhen(false)] out T item)
        {
            return this.items.TryGetValue(id, out item);
        }

        public bool Contains(T item)
        {
            var id = this.GetId(item);
            return this.items.ContainsKey(id);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return items.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }
    }
}
