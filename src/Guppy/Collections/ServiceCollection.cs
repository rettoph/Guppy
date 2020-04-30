using Guppy.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xxHashSharp;

namespace Guppy.Collections
{
    public class ServiceCollection<TService> : Service, IEnumerable<TService>
        where TService : IService
    {
        #region Private Fields
        private Dictionary<Guid, TService> _dictionary;
        private List<TService> _list;
        private ServiceProvider _provider;
        #endregion

        #region Events
        public event EventHandler<TService> OnAdded;
        public event EventHandler<TService> OnRemoved;
        #endregion

        #region Constructor
        public ServiceCollection()
        {
            _dictionary = new Dictionary<Guid, TService>();
            _list = new List<TService>();
        }
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            _provider = provider;
        }

        protected override void Dispose()
        {
            base.Dispose();

            this.TryClear();
        }
        #endregion

        #region Helper Methods
        public virtual Boolean CanAdd(TService item)
        {
            return !_dictionary.ContainsKey(item.Id);
        }

        public virtual Boolean CanRemove(TService item)
        {
            return _dictionary.ContainsKey(item.Id);
        }
        #endregion

        #region Factory Methods
        protected virtual TService Create(ServiceProvider provider, Type serviceType)
        {
            ExceptionHelper.ValidateAssignableFrom<TService>(serviceType);

            var item = (TService)provider.GetService(serviceType);
            this.TryAdd(item);
            return item;
        }
        protected virtual TService Create(ServiceProvider provider, UInt32 id)
        {
            var item = provider.GetService<TService>(id);
            this.TryAdd(item);
            return item;
        }
        public TService Create(UInt32 id)
        {
            return this.Create(_provider, id);
        }
        public TService Create(String handle)
        {
            return this.Create(xxHash.CalculateHash(Encoding.UTF8.GetBytes(handle)));
        }
        public TService Create(Type serviceType)
        {
            return this.Create(_provider, serviceType);
        }
        public T Create<T>(UInt32 id)
            where T : TService
        {
            return (T)this.Create(_provider, id);
        }
        public T Create<T>(String handle)
            where T : TService
        {
            return this.Create<T>(xxHash.CalculateHash(Encoding.UTF8.GetBytes(handle)));
        }
        public T Create<T>(Type serviceType)
            where T : TService
        {
            return (T)this.Create(serviceType);
        }

        public T Create<T>()
            where T : TService
        {
            return this.Create<T>(typeof(T));
        }
        #endregion

        #region ICollection Implementation
        public Int32 Count => _list.Count;

        public void TryAdd(TService item)
        {
            if(this.CanAdd(item))
            {
                this.Add(item);
                this.OnAdded?.Invoke(this, item);
            }
        }

        protected virtual void Add(TService item)
        {
            _list.Add(item);
            _dictionary.Add(item.Id, item);

            item.OnDisposed += this.HandleItemDisposed;
        }

        public Boolean TryRemove(TService item)
        {
            if(this.CanRemove(item))
            {
                this.Remove(item);
                this.OnRemoved?.Invoke(this, item);
            }

            return false;
        }

        protected virtual void Remove(TService item)
        {
            _list.Remove(item);
            _dictionary.Remove(item.Id);

            item.OnDisposed -= this.HandleItemDisposed;
        }

        public void TryClear()
        {
            this.Clear();
        }

        protected virtual void Clear()
        {
            while (this.Count > 0) // Auto remove all elements
                this.TryRemove(_list.First());
        }

        public bool Contains(TService item)
        {
            return _list.Contains(item);
        }

        public IEnumerator<TService> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
        #endregion

        #region Event Handlers
        private void HandleItemDisposed(object sender, EventArgs e)
        {
            this.TryRemove((TService)sender);
        }
        #endregion
    }
}
