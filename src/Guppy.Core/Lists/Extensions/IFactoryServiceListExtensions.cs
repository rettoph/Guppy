using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Lists.Interfaces;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Lists.Extensions
{
    public static class IFactoryServiceListExtensions
    {
        #region Create Methods
        public static T Create<T>(this IFactoryServiceList list, Type descriptorType, Action<T, ServiceProvider, ServiceDescriptor> setup = null)
            where T : class, IService
        {
            ExceptionHelper.ValidateAssignableFrom<T>(list.ListType);

            var instance = list.Provider.GetService<T>(descriptorType, setup);
            list.TryAdd(instance);

            return instance;
        }

        public static T Create<T>(this IFactoryServiceList list, UInt32 descriptorId, Action<T, ServiceProvider, ServiceDescriptor> setup = null)
            where T : class, IService
        {
            ExceptionHelper.ValidateAssignableFrom<T>(list.ListType);

            var instance = list.Provider.GetService<T>(descriptorId, setup);
            list.TryAdd(instance);

            return instance;
        }

        public static T Create<T>(this IFactoryServiceList list, String descriptorName, Action<T, ServiceProvider, ServiceDescriptor> setup = null)
            where T : class, IService
        {
            ExceptionHelper.ValidateAssignableFrom<T>(list.ListType);

            var instance = list.Provider.GetService<T>(descriptorName, setup);
            list.TryAdd(instance);

            return instance;
        }
        #endregion

        #region GetOrCreateById Methods
        public static T GetOrCreateById<T>(this IFactoryServiceList list, Guid id, UInt32 descriptorId)
            where T : class, IService
        {
            ExceptionHelper.ValidateAssignableFrom<T>(list.ListType);

            var instance = list.GetById<T>(id);
            if (instance != default(T))
                return instance;

            return list.Create<T>(descriptorId, (i, p, d) => i.Id = id);
        }

        public static T GetOrCreateById<T>(this IFactoryServiceList list, Guid id, String descriptorName)
            where T : class, IService
        {
            ExceptionHelper.ValidateAssignableFrom<T>(list.ListType);

            var instance = list.GetById<T>(id);
            if (instance != default(T))
                return instance;

            return list.Create<T>(descriptorName, (i, p, d) => i.Id = id);
        }

        public static T GetOrCreateById<T>(this IFactoryServiceList list, Guid id, Type descriptorType)
            where T : class, IService
        {
            ExceptionHelper.ValidateAssignableFrom<T>(list.ListType);

            var instance = list.GetById<T>(id);
            if (instance != default(T))
                return instance;

            return list.Create<T>(descriptorType, (i, p, d) => i.Id = id);
        }
        #endregion
    }
}
