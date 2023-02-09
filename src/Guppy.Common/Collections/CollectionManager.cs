using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Collections
{
    public partial class CollectionManager<T> : ICollectionManager<T>
    {
        private IDictionary<Type, Manager> _managers = new Dictionary<Type, Manager>();
        private IList<T> _items = new List<T>();

        public IEnumerable<IEnumerable> Collections => _managers.Values.Select(x => x.Collection);

        public ICollection<TItem> Collection<TItem>()
        {
            if(!_managers.TryGetValue(typeof(TItem), out var manager))
            {
                throw new NotImplementedException();
            }

            if(manager is not Manager<TItem> casted)
            {
                throw new NotImplementedException();
            }

            return casted.Collection;
        }

        public ICollectionManager<T> Attach<TItem>(ICollection<TItem> collection)
        {
            var manager = new Manager<TItem>(collection);
            _managers.Add(manager.Type, manager);

            foreach(T item in this)
            {
                manager.TryAdd(item);
            }
            return this;
        }

        public ICollectionManager<T> Detach<TItem>(ICollection<TItem> collection)
        {
            _managers.Remove(typeof(TItem));
            return this;
        }

        public virtual void Add(T item)
        {
            _items.Add(item);

            foreach(var manager in _managers.Values)
            {
                manager.TryAdd(item);
            }
        }

        public virtual bool Remove(T item)
        {
            if(!_items.Remove(item))
            {
                return false;
            }

            foreach (var manager in _managers.Values)
            {
                manager.TryRemove(item);
            }

            return true;
        }

        public void Clear()
        {
            _items.Clear();

            foreach (var manager in _managers.Values)
            {
                manager.Clear();
            }
        }

        public ICollectionManager<T> AddRange(IEnumerable<T> items)
        {
            foreach(T item in items)
            {
                this.Add(item);
            }

            return this;
        }
    }
}
