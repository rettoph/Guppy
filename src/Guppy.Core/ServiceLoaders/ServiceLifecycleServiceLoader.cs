using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.Collections;
using Guppy.Extensions.DependencyInjection;
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
        private ServiceProvider _provider;

        public void ConfigureServices(ServiceCollection services)
        {
            services.AddBuilder<IService>((s, p) => s.TryCreate(p));

            services.AddConfiguration<IService>((s, p, sd) =>
            {
                s.ServiceDescriptor = sd;
                s.TryPreInitialize(p);

                s.OnReleased += this.HandleServiceReleased;
            }, -10);

            services.AddConfiguration<IService>((s, p, sd) => s.TryInitialize(p), 10);

            services.AddConfiguration<IService>((s, p, sd) => s.TryPostInitialize(p), 20);
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            _provider = provider;
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

            sender.ServiceDescriptor.Factory.Return(sender);
        }
        #endregion
    }
}
