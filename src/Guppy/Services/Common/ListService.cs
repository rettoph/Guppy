using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Services.Common
{
    public abstract class ListService<TKey, T> : CollectionService<TKey, T>, IListService<TKey, T>
        where TKey : notnull
        where T : class
    {
        private IServiceProvider _provider;

        public event OnEventDelegate<IListService<TKey, T>, T>? OnItemAdded;
        public event OnEventDelegate<IListService<TKey, T>, T>? OnItemRemoved;

        public ListService(IServiceProvider provider)
        {
            _provider = provider;
        }

        public bool TryAdd(T item)
        {
            if(this.TryAdd(this.GetKey(item), item))
            {
                this.OnItemAdded?.Invoke(this, item);

                return true;
            }

            return false;
        }

        public bool TryRemove(T item)
        {
            if (this.TryRemove(this.GetKey(item), item))
            {
                this.OnItemRemoved?.Invoke(this, item);

                return true;
            }

            return false;
        }

        protected virtual bool TryAdd(TKey key, T item)
        {
            return this.items.TryAdd(key, item);
        }

        protected virtual bool TryRemove(TKey key, T item)
        {
            return this.items.Remove(key);
        }

        public bool TryCreate<TItem>([MaybeNullWhen(false)] out TItem item)
            where TItem : class, T
        {
            if(this.TryCreate(this.GetFactoryServiceProvider(_provider), out item))
            {
                return this.TryAdd(item);
            }

            return false;
        }

        public TItem Create<TItem>()
            where TItem : class, T
        {
            var item = this.Create<TItem>(this.GetFactoryServiceProvider(_provider));
            this.TryAdd(item);

            return item;
        }

        public bool TryCreate(Type type, [MaybeNullWhen(false)] out T item)
        {
            if (this.TryCreate(this.GetFactoryServiceProvider(_provider), type, out item))
            {
                return this.TryAdd(item);
            }

            return false;
        }

        public T Create(Type type)
        {
            var item = this.Create(this.GetFactoryServiceProvider(_provider), type);
            this.TryAdd(item);

            return item;
        }

        public bool TryCreate([MaybeNullWhen(false)] out T item)
        {
            if (this.TryCreate<T>(this.GetFactoryServiceProvider(_provider), out item))
            {
                return this.TryAdd(item);
            }

            return false;
        }

        public T Create()
        {
            var item = this.Create<T>(this.GetFactoryServiceProvider(_provider));
            this.TryAdd(item);

            return item;
        }

        protected virtual bool TryCreate<TItem>(IServiceProvider provider, [MaybeNullWhen(false)] out TItem item)
            where TItem : class, T
        {
            item = provider.GetService<TItem>();
            return item is not null;
        }

        protected virtual TItem Create<TItem>(IServiceProvider provider)
            where TItem : class, T
        {
            return provider.GetRequiredService<TItem>();
        }

        protected virtual bool TryCreate(IServiceProvider provider, Type type, [MaybeNullWhen(false)] out T item)
        {
            item = provider.GetService(type) as T;
            return item is not null;
        }

        protected virtual T Create(IServiceProvider provider, Type type)
        {
            return (T)provider.GetRequiredService(type);
        }

        protected virtual IServiceProvider GetFactoryServiceProvider(IServiceProvider provider)
        {
            return provider;
        }
    }
}
