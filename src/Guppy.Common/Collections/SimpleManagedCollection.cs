using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Collections
{
    public abstract class SimpleManagedCollection<T> : IManagedCollection, IEnumerable<T>
        where T : notnull
    {
        public Type Type { get; } = typeof(T);

        protected readonly List<T> items;

        public SimpleManagedCollection()
        {
            this.items = new List<T>();
        }

        void IManagedCollection.Initialize(CollectionManager manager)
        {
            this.Initialize(manager);
        }

        protected virtual void Initialize(CollectionManager manager)
        {
            //
        }

        public bool TryAdd(object item)
        {
            if(item is T casted)
            {
                return this.Add(casted);
            }

            return false;
        }

        public bool TryRemove(object item)
        {
            if (item is T casted)
            {
                return this.Remove(casted);
            }

            return false;
        }

        protected virtual bool Add(T item)
        {
            this.items.Add(item);
            return true;
        }
        protected virtual bool Remove(T item)
        {
            return this.items.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)items).GetEnumerator();
        }
    }
}
