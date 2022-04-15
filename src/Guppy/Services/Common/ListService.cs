using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Services.Common
{
    public abstract class ListService<TId, T> : CollectionService<TId, T>, IListService<TId, T>
        where T : class
    {
        private IServiceProvider _provider;

        public event OnEventDelegate<IListService<TId, T>, T>? OnItemAdded;
        public event OnEventDelegate<IListService<TId, T>, T>? OnItemRemoved;

        public ListService(IServiceProvider provider)
        {
            _provider = provider;
        }

        public virtual bool TryAdd(T item)
        {
            if(this.items.TryAdd(this.GetId(item), item))
            {
                this.OnItemAdded?.Invoke(this, item);

                return true;
            }

            return false;
        }

        public virtual bool TryCreate<TItem>(out TItem? item)
            where TItem : class, T
        {
            item = this.Create<TItem>(_provider);

            if(item is null)
            {
                return false;
            }

            return this.TryAdd(item);
        }

        public virtual bool TryRemove(T item)
        {
            if (this.items.Remove(this.GetId(item)))
            {
                this.OnItemRemoved?.Invoke(this, item);

                return true;
            }

            return false;
        }

        protected virtual TItem? Create<TItem>(IServiceProvider provider)
            where TItem : class, T
        {
            return provider.GetService<TItem>();
        }
    }
}
