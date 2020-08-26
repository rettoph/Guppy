using Guppy.DependencyInjection;
using Guppy.Extensions.System;
using Guppy.Interfaces;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using xxHashSharp;
using Guppy.Extensions.DependencyInjection;

namespace Guppy.Collections
{
    /// <summary>
    /// Extension of basic ServiceCollection that will
    /// automatically implement factory methods for
    /// internal instances.
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public class FactoryCollection<TService> : ServiceCollection<TService>
        where TService : IService
    {
        #region Private Fields
        private ServiceProvider _provider;
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            _provider = provider;
        }
        #endregion

        #region Factory Methods
        protected virtual TService Create(ServiceProvider provider, Type serviceType, Action<TService, ServiceProvider, ServiceDescriptor> setup = null)
        {
            ExceptionHelper.ValidateAssignableFrom<TService>(serviceType);

            var item = (TService)provider.GetService(serviceType, (i, p, c) => setup?.Invoke((TService)i, p, c));
            this.TryAdd(item);
            return item;
        }
        protected virtual TService Create(ServiceProvider provider, UInt32 id, Action<TService, ServiceProvider, ServiceDescriptor> setup = null)
        {
            var item = provider.GetService<TService>(id, setup);
            this.TryAdd(item);
            return item;
        }
        public TService Create(UInt32 configurationId, Action<TService, ServiceProvider, ServiceDescriptor> setup = null)
        {
            return this.Create(_provider, configurationId, setup);
        }
        public TService Create(String handle, Action<TService, ServiceProvider, ServiceDescriptor> setup = null)
        {
            return this.Create(ServiceDescriptor.GetId(handle), setup);
        }
        public TService Create(Type serviceType, Action<TService, ServiceProvider, ServiceDescriptor> setup = null)
        {
            return this.Create(_provider, serviceType, setup);
        }
        public T Create<T>(UInt32 configurationId, Action<T, ServiceProvider, ServiceDescriptor> setup = null)
            where T : TService
        {
            return (T)this.Create(_provider, configurationId, (i, p, c) => setup?.Invoke((T)i, p, c));
        }
        public T Create<T>(String handle, Action<T, ServiceProvider, ServiceDescriptor> setup = null)
            where T : TService
        {
            return this.Create<T>(ServiceDescriptor.GetId(handle), setup);
        }
        public T Create<T>(Type serviceType, Action<T, ServiceProvider, ServiceDescriptor> setup = null)
            where T : TService
        {
            return (T)this.Create(serviceType, (i, p, c) => setup?.Invoke((T)i, p, c));
        }

        public T Create<T>(Action<T, ServiceProvider, ServiceDescriptor> setup = null)
            where T : TService
        {
            return this.Create<T>(typeof(T), setup);
        }




        public TService GetOrCreateById(Guid id, UInt32 configurationId, Action<TService, ServiceProvider, ServiceDescriptor> setup = null)
        {
            if (this.ContainsId(id))
                return this.GetById(id);
            else
                return this.Create(_provider, configurationId, (i, p, c) =>
                {
                    i.Id = id;
                    setup?.Invoke(i, p, c);
                });
        }
        public TService GetOrCreateById(Guid id, String handle, Action<TService, ServiceProvider, ServiceDescriptor> setup = null)
        {
            if (this.ContainsId(id))
                return this.GetById(id);
            else
                return this.Create(ServiceDescriptor.GetId(handle), (i, p, c) =>
                {
                    i.Id = id;
                    setup?.Invoke(i, p, c);
                });
        }
        public TService GetOrCreateById(Guid id, Type serviceType, Action<TService, ServiceProvider, ServiceDescriptor> setup = null)
        {
            if (this.ContainsId(id))
                return this.GetById(id);
            else
                return this.Create(_provider, serviceType, (i, p, c) =>
                {
                    i.Id = id;
                    setup?.Invoke(i, p, c);
                });
        }
        public T GetOrCreateById<T>(Guid id, UInt32 configurationId, Action<T, ServiceProvider, ServiceDescriptor> setup = null)
            where T : TService
        {
            if (this.ContainsId(id))
                return this.GetById<T>(id);
            else
                return (T)this.Create(_provider, configurationId, (i, p, c) =>
                {
                    i.Id = id;
                    setup?.Invoke((T)i, p, c);
                });
        }
        public T GetOrCreateById<T>(Guid id, String handle, Action<T, ServiceProvider, ServiceDescriptor> setup = null)
            where T : TService
        {
            if (this.ContainsId(id))
                return this.GetById<T>(id);
            else
                return this.Create<T>(ServiceDescriptor.GetId(handle), (i, p, c) =>
                {
                    i.Id = id;
                    setup?.Invoke(i, p, c);
                });
        }
        public T GetOrCreateById<T>(Guid id, Type serviceType, Action<T, ServiceProvider, ServiceDescriptor> setup = null)
            where T : TService
        {
            if (this.ContainsId(id))
                return this.GetById<T>(id);
            else
                return this.Create<T>(serviceType, (i, p, c) =>
                {
                    i.Id = id;
                    setup?.Invoke(i, p, c);
                });
        }

        public T GetOrCreateById<T>(Guid id, Action<T, ServiceProvider, ServiceDescriptor> setup = null)
            where T : TService
        {
            if (this.ContainsId(id))
                return this.GetById<T>(id);
            else
                return this.Create<T>(typeof(T), (i, p, c) =>
                {
                    i.Id = id;
                    setup?.Invoke(i, p, c);
                });
        }
        #endregion
    }
}
