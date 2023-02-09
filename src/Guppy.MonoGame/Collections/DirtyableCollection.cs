using Guppy.Common.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Collections
{
    public abstract class DirtyableCollection<T> : ICollection<T>
        where T : notnull
    {
        private readonly List<T> _cache = new List<T>();
        protected readonly List<T> items = new List<T>();

        protected bool dirty;

        public int Count => ((ICollection<T>)items).Count;

        public bool IsReadOnly => ((ICollection<T>)items).IsReadOnly;

        public virtual void Add(T item)
        {
            this.items.Add(item);
            this.dirty = true;
        }

        public virtual void Clear()
        {
            while(this.Any())
            {
                this.Remove(this.First());
            }
        }

        public bool Contains(T item)
        {
            return this.items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.items.CopyTo(array, arrayIndex);
        }

        public virtual void EnsureClean()
        {
            if(!this.dirty)
            {
                return;
            }

            _cache.Clear();
            _cache.AddRange(this.items);

            var clean = this.Clean(_cache);

            this.items.Clear();
            this.items.AddRange(clean);
  
            this.dirty = false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (this.dirty)
            {
                this.EnsureClean();
            }

            return this.items.GetEnumerator();
        }

        public virtual bool Remove(T item)
        {
            if(this.items.Remove(item))
            {
                this.dirty = true;
                return true;
            }
            
            return false;
        }

        protected abstract IEnumerable<T> Clean(IEnumerable<T> items);

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
