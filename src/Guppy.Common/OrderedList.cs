using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public class OrderedList<T> : IList<Orderable<T>>, IEnumerable<T>
    {
        private IList<Orderable<T>> _items;
        private List<T> _ordered;
        private bool _dirty;

        public Orderable<T> this[int index] { get => _items[index]; set => _items[index] = value; }

        public int Count => _items.Count;

        public bool IsReadOnly => _items.IsReadOnly;

        public OrderedList()
        {
            _items = new List<Orderable<T>>();
            _ordered = new List<T>();
        }

        public void Add(Orderable<T> item)
        {
            _dirty = true;
            _items.Add(item);
        }

        public void Add(T item, int order)
        {
            this.Add(new Orderable<T>(item, order));
        }

        public void Clear()
        {
            _dirty = true;
            _items.Clear();
        }

        public bool Contains(Orderable<T> item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(Orderable<T>[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        IEnumerator<Orderable<T>> IEnumerable<Orderable<T>>.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public int IndexOf(Orderable<T> item)
        {
            return _items.IndexOf(item);
        }

        public void Insert(int index, Orderable<T> item)
        {
            _dirty = true;
            _items.Insert(index, item);
        }

        public bool Remove(Orderable<T> item)
        {
            _dirty = true;
            return _items.Remove(item);
        }

        public bool Remove(T item)
        {
            var target = _items.FirstOrDefault(x => x.Instance?.Equals(item) ?? false);

            if(target is null)
            {
                return false;
            }

            return this.Remove(target);
        }

        public void RemoveAt(int index)
        {
            _dirty = true;
            _items.RemoveAt(index);
        }
        public IEnumerator<T> GetEnumerator()
        {
            if (_dirty)
            {
                _ordered.Clear();
                _ordered.AddRange(_items.OrderBy(x => x.Order).Select(x => x.Instance));
                _dirty = false;
            }

            return _ordered.GetEnumerator();
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
