using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Services.Common
{
    public abstract class CollectionService<TKey, T> : ICollectionService<TKey, T>
        where TKey : notnull
        where T : class
    {
        protected Dictionary<TKey, T> items;

        public virtual T this[TKey key] => this.items[key];

        public CollectionService(int capacity = 0)
        {
            this.items = new Dictionary<TKey, T>(capacity);
        }
        public CollectionService(IEnumerable<T> items)
        {
            this.items = new Dictionary<TKey, T>(items.Select(x => new KeyValuePair<TKey, T>(this.GetKey(x), x)));
        }

        protected abstract TKey GetKey(T item);

        public virtual bool TryGet(TKey key, [MaybeNullWhen(false)] out T item)
        {
            return this.items.TryGetValue(key, out item);
        }

        public virtual T Get(TKey key)
        {
            return this.items[key];
        }

        public virtual bool Contains(T item)
        {
            var key = this.GetKey(item);
            return this.items.ContainsKey(key);
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
