using Guppy.Common.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Collections
{
    public abstract class DirtyableCollection<T> : SimpleManagedCollection<T>
        where T : notnull
    {
        private CollectionManager _manager = default!;

        protected bool dirty;

        protected override void Initialize(CollectionManager manager)
        {
            base.Initialize(manager);

            _manager = manager;
        }

        public virtual void EnsureClean()
        {
            if(!this.dirty)
            {
                return;
            }

            this.items.Clear();
            var clean = this.Clean(_manager.Items<T>());
            this.items.AddRange(clean);

            this.dirty = false;
        }

        protected abstract IEnumerable<T> Clean(IEnumerable<T> items);

        protected override bool Add(T item)
        {
            if(base.Add(item))
            {
                this.dirty = true;
                return true;
            }

            return false;
        }

        protected override bool Remove(T item)
        {
            if (base.Remove(item))
            {
                this.dirty = true;
                return true;
            }

            return false;
        }
    }
}
