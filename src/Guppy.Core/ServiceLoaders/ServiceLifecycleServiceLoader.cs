using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.Collections;
using Guppy.Interfaces;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.ServiceLoaders
{
    [AutoLoad(Int32.MaxValue)]
    internal class ServiceLifecycleServiceLoader : IServiceLoader
    {
        public void RegisterServices(ServiceCollection services)
        {
            services.AddBuilder<IService>((s, p, c) => s.TryCreate(p));

            services.AddSetup<IService>((s, p, sd) =>
            {
                s.ServiceConfiguration = sd;
                s.TryPreInitialize(p);

                s.OnReleased += this.HandleServiceReleased;
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
        /// When an IService instance is disposed, we should automatically
        /// return the instance to the ServiceTypeDescriptor pool.
        /// </summary>
        /// <param name="sender"></param>
        private void HandleServiceReleased(IService sender)
        {
            sender.OnReleased -= this.HandleServiceReleased;

            sender.ServiceConfiguration.Factory.Pools[sender].TryReturn(sender);
        }
        #endregion
    }
}
