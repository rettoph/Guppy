using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Collections
{
    public abstract class CollectionManager
    {
        public abstract IEnumerable<TItem> Items<TItem>();
    }

    public class CollectionManager<T> : CollectionManager, IEnumerable<T>
        where T : notnull
    {
        private readonly IList<T> _items;
        private readonly IManagedCollection[] _collections;

        public CollectionManager(IEnumerable<T> items, params IManagedCollection[] collections)
        {
            _items = new List<T>();
            _collections = collections;

            foreach(IManagedCollection collection in _collections)
            {
                collection.Initialize(this);
            }

            this.AddRange(items);
        }

        public override IEnumerable<TItem> Items<TItem>()
        {
            return _items.OfType<TItem>();
        }

        public int Add(T item)
        {
            _items.Add(item);

            int count = 0;
            foreach (IManagedCollection collection in _collections)
            {
                if (collection.TryAdd(item))
                {
                    count++;
                }
            }

            return count;
        }

        public int Remove(T item)
        {
            int count = 0;

            if (!_items.Remove(item))
            {
                return count;
            }

            foreach (IManagedCollection collection in _collections)
            {
                if (collection.TryRemove(item))
                {
                    count++;
                }
            }

            return count;
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach(T item in items)
            {
                this.Add(item);
            }
        }

        public void Clear()
        {
            while(_items.Any())
            {
                this.Remove(_items[0]);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public TManagedCollection GetCollection<TManagedCollection>()
            where TManagedCollection : IManagedCollection
        {
            foreach(var collection in _collections)
            {
                if(collection is TManagedCollection casted)
                {
                    return casted;
                }
            }

            throw new KeyNotFoundException();
        }
    }
}
