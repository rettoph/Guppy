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
            services.AddBuilder<IService>((s, p, c) => s.TryPreCreate(p), -10);
            services.AddBuilder<IService>((s, p, c) => s.TryCreate(p), 0);
            services.AddBuilder<IService>((s, p, c) => s.TryPostCreate(p), 10);

            services.AddSetup<IService>((s, p, sd) =>
            {
                s.ServiceConfiguration = sd;
                s.TryPreInitialize(p);

                s.OnStatus[ServiceStatus.NotReady] += this.HandleServiceNotReady;
            }, -10);

            services.AddSetup<IService>((s, p, sd) => s.TryInitialize(p), 10);

            services.AddSetup<IService>((s, p, sd) => s.TryPostInitialize(p), 20);
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
