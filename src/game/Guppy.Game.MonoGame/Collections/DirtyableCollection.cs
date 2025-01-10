using System.Collections;

namespace Guppy.Game.MonoGame.Collections
{
    public abstract class DirtyableCollection<T> : ICollection<T>
        where T : notnull
    {
        private readonly List<T> _cache = [];
        protected readonly List<T> items = [];

        protected bool dirty;

        public int Count => ((ICollection<T>)this.items).Count;

        public bool IsReadOnly => ((ICollection<T>)this.items).IsReadOnly;

        public virtual void Add(T item)
        {
            this._cache.Add(item);

            this.dirty = true;
        }

        public virtual void Clear()
        {
            this._cache.Clear();
            this.dirty = true;
        }

        public bool Contains(T item) => this.items.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => this.items.CopyTo(array, arrayIndex);

        public virtual void EnsureClean()
        {
            if (!this.dirty)
            {
                return;
            }

            var clean = this.Clean(this._cache);

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
            if (this._cache.Remove(item))
            {
                this.dirty = true;
                return true;
            }

            return false;
        }

        protected abstract IEnumerable<T> Clean(IEnumerable<T> items);

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}