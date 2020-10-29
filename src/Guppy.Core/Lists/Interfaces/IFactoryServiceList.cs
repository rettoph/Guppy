using Guppy.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Lists.Delegates;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Lists.Interfaces
{
    public interface IFactoryServiceList<TService> : IServiceList<TService>
        where TService : class, IService
    {
        #region Events
        event ItemDelegate<TService> OnCreated;
        #endregion

        #region Create Methods
        TService Create(UInt32 descriptorId, Action<TService, ServiceProvider, ServiceDescriptor> setup = null, Guid? id = null);

        TService Create(Type descriptorType, Action<TService, ServiceProvider, ServiceDescriptor> setup = null, Guid? id = null);

        TService Create(String descriptorName, Action<TService, ServiceProvider, ServiceDescriptor> setup = null, Guid? id = null);

        TService Create(Action<TService, ServiceProvider, ServiceDescriptor> setup = null, Guid? id = null);

        T Create<T>(UInt32 descriptorId, Action<T, ServiceProvider, ServiceDescriptor> setup = null, Guid? id = null)
            where T : class, TService;

        T Create<T>(Type descriptorType, Action<T, ServiceProvider, ServiceDescriptor> setup = null, Guid? id = null)
            where T : class, TService;

        T Create<T>(String descriptorName, Action<T, ServiceProvider, ServiceDescriptor> setup = null, Guid? id = null)
            where T : class, TService;

        T Create<T>(Action<T, ServiceProvider, ServiceDescriptor> setup = null, Guid? id = null)
            where T : class, TService;
        #endregion

        #region GetOrCreateById Methods
        TService GetOrCreateById(Guid id, UInt32 descriptorId);

        TService GetOrCreateById(Guid id, String descriptorName);

        TService GetOrCreateById(Guid id, Type descriptorType);

        TService GetOrCreateById(Guid id);

        T GetOrCreateById<T>(Guid id, UInt32 descriptorId)
            where T : class, TService;

        T GetOrCreateById<T>(Guid id, String descriptorName)
            where T : class, TService;

        T GetOrCreateById<T>(Guid id, Type descriptorType)
            where T : class, TService;

        T GetOrCreateById<T>(Guid id)
            where T : class, TService;
        #endregion
    }
}
