using Guppy.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Lists.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;
using Guppy.Lists.Delegates;
using Guppy.Exceptions;

namespace Guppy.Lists
{
    public partial class ServiceList<TService> : IFactoryServiceList<TService>
        where TService : class, IService
    {
        #region Events
        public event ItemDelegate<TService> OnCreated;
        #endregion

        #region Create Methods
        public T Create<T>(UInt32 descriptorId, Action<T, ServiceProvider, ServiceDescriptor> setup = null)
            where T : class, TService
        {
            var instance = this.provider.GetService<T>(descriptorId, setup);
            this.OnCreated?.Invoke(instance);

            if(!this.AutoFill)
                this.TryAdd(instance);

            return instance;
        }

        public T Create<T>(Type descriptorType, Action<T, ServiceProvider, ServiceDescriptor> setup = null)
            where T : class, TService
        {
            try
            {
                return this.Create<T>(ServiceDescriptor.GetId(descriptorType), setup);
            }
            catch(ServiceIdUnknownException e)
            {
                throw new ServiceTypeUnknown(descriptorType, e);
            }
        }

        public T Create<T>(String descriptorName, Action<T, ServiceProvider, ServiceDescriptor> setup = null)
            where T : class, TService
        {
            try
            {
                return this.Create<T>(ServiceDescriptor.GetId(descriptorName), setup);
            }
            catch (ServiceIdUnknownException e)
            {
                throw new ServiceNameUnknown(descriptorName, e);
            }
        }

        public T Create<T>(Action<T, ServiceProvider, ServiceDescriptor> setup = null)
            where T : class, TService
                => this.Create<T>(typeof(T), setup);

        public TService Create(UInt32 descriptorId, Action<TService, ServiceProvider, ServiceDescriptor> setup = null)
            => this.Create<TService>(descriptorId, setup);

        public TService Create(Type descriptorType, Action<TService, ServiceProvider, ServiceDescriptor> setup = null)
            => this.Create(ServiceDescriptor.GetId(descriptorType), setup);

        public TService Create(String descriptorName, Action<TService, ServiceProvider, ServiceDescriptor> setup = null)
            => this.Create(ServiceDescriptor.GetId(descriptorName), setup);

        public TService Create(Action<TService, ServiceProvider, ServiceDescriptor> setup = null)
             => this.Create(typeof(TService), setup);
        #endregion

        #region GetOrCreateById Methods
        public T GetOrCreateById<T>(Guid id, UInt32 descriptorId)
            where T : class, TService
        {
            var instance = this.GetById<T>(id);
            if (instance != default(T))
                return instance;

            return this.Create<T>(descriptorId, (i, p, d) => i.Id = id);
        }

        public T GetOrCreateById<T>(Guid id, String descriptorName)
            where T : class, TService
                => this.GetOrCreateById<T>(id, ServiceDescriptor.GetId(descriptorName));

        public T GetOrCreateById<T>(Guid id, Type descriptorType)
            where T : class, TService
                => this.GetOrCreateById<T>(id, ServiceDescriptor.GetId(descriptorType));

        public T GetOrCreateById<T>(Guid id)
            where T : class, TService
                => this.GetOrCreateById<T>(id, typeof(TService));

        public TService GetOrCreateById(Guid id, UInt32 descriptorId)
            => this.GetOrCreateById<TService>(id, descriptorId);

        public TService GetOrCreateById(Guid id, String descriptorName)
            => this.GetOrCreateById(id, ServiceDescriptor.GetId(descriptorName));

        public TService GetOrCreateById(Guid id, Type descriptorType)
            => this.GetOrCreateById(id, ServiceDescriptor.GetId(descriptorType));

        public TService GetOrCreateById(Guid id)
            => this.GetOrCreateById(id, ServiceDescriptor.GetId(typeof(TService)));
        #endregion
    }
}
