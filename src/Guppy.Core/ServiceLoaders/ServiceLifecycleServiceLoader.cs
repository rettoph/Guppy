using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.System.Collections;
using Guppy.Interfaces;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guppy.Enums;
using Guppy.Extensions.DependencyInjection;

namespace Guppy.ServiceLoaders
{
    [AutoLoad(Int32.MaxValue)]
    internal class ServiceLifecycleServiceLoader : IServiceLoader
    {
        public void RegisterServices(GuppyServiceCollection services)
        {
            services.RegisterBuilder<IService>((s, p, sd) =>
            {
                s.OnStatus[ServiceStatus.NotInitialized] += this.HandleServiceNotReady;
            }, Guppy.Core.Constants.Priorities.Create);

            services.RegisterBuilder<IService>((s, p, sd) => s.TryPreCreate(p) , Guppy.Core.Constants.Priorities.PreCreate);
            services.RegisterBuilder<IService>((s, p, sd) => s.TryCreate(p)    , Guppy.Core.Constants.Priorities.Create);
            services.RegisterBuilder<IService>((s, p, sd) => s.TryPostCreate(p), Guppy.Core.Constants.Priorities.PostCreate);

            services.RegisterSetup<IService>((s, p, sd) =>
            {
                s.ServiceConfiguration = sd;
            }, Int32.MinValue);

            services.RegisterSetup<IService>((s, p, sd) => s.TryPreInitialize(p) , Guppy.Core.Constants.Priorities.PreInitialize);
            services.RegisterSetup<IService>((s, p, sd) => s.TryInitialize(p)    , Guppy.Core.Constants.Priorities.Initialize);
            services.RegisterSetup<IService>((s, p, sd) => s.TryPostInitialize(p), Guppy.Core.Constants.Priorities.PostInitialize);
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // 
        }

        #region Event Handlers
        /// <summary>
        /// When an IService instance is marked not ready, we should automatically
        /// return the instance to the ServiceTypeDescriptor pool.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="old"></param>
        /// <param name="value"></param>
        private void HandleServiceNotReady(IService sender, ServiceStatus old, ServiceStatus value)
        {
            if(old == ServiceStatus.PostReleasing)
                sender.ServiceConfiguration.TypeFactory.TryReturnToPool(sender);
        }
        #endregion
    }
}
