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

namespace Guppy.ServiceLoaders
{
    [AutoLoad(Int32.MaxValue)]
    internal class ServiceLifecycleServiceLoader : IServiceLoader
    {
        public void RegisterServices(ServiceCollection services)
        {
            services.AddBuilder<IService>((s, p, c) => s.TryPreCreate(p), GuppyCoreConstants.Priorities.PreCreate);
            services.AddBuilder<IService>((s, p, c) => s.TryCreate(p), GuppyCoreConstants.Priorities.Create);
            services.AddBuilder<IService>((s, p, c) => s.TryPostCreate(p), GuppyCoreConstants.Priorities.PostCreate);

            services.AddSetup<IService>((s, p, sd) =>
            {
                s.ServiceConfiguration = sd;
                s.TryPreInitialize(p);

                s.OnStatus[ServiceStatus.NotReady] += this.HandleServiceNotReady;
            }, GuppyCoreConstants.Priorities.PreInitialize);

            services.AddSetup<IService>((s, p, sd) => s.TryInitialize(p), GuppyCoreConstants.Priorities.Initialize);

            services.AddSetup<IService>((s, p, sd) => s.TryPostInitialize(p), GuppyCoreConstants.Priorities.PostInitialize);
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // 
        }

        #region Event Handlers
        /// <summary>
        /// When an IService instance is marked not ready, we should automatically
        /// return the instance to the ServiceTypeDescriptor pool.
        /// </summary>
        /// <param name="sender"></param>
        private void HandleServiceNotReady(IService sender)
        {
            sender.OnStatus[ServiceStatus.NotReady] -= this.HandleServiceNotReady;

            sender.ServiceConfiguration.Factory.Pools[sender].TryReturn(sender);
        }
        #endregion
    }
}
