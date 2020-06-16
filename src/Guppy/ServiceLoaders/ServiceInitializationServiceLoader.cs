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
    internal class ServiceInitializationServiceLoader : IServiceLoader
    {
        private ServiceProvider _provider;

        public void ConfigureServices(ServiceCollection services)
        {
            services.AddConfiguration<IService>((s, p, c) =>
            {
                s.ServiceConfiguration = c;
                s.TryPreInitialize(p);

                s.OnDisposed += this.HandleServiceDisposed;
            }, -10);

            services.AddConfiguration<IService>((s, p, f) => s.TryInitialize(p), 10);

            services.AddConfiguration<IService>((s, p, f) => s.TryPostInitialize(p), 20);
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
        private void HandleServiceDisposed(IService sender)
        {
            sender.OnDisposed -= this.HandleServiceDisposed;

            _provider.GetServiceTypeDescriptor(sender.GetType()).Release(sender);
        }
        #endregion
    }
}
