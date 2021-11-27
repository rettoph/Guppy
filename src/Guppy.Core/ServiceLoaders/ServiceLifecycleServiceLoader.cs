using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guppy.Enums;
using Guppy.Extensions.DependencyInjection;
using Guppy.DependencyInjection.Actions;
using Guppy.DependencyInjection.ServiceConfigurations;
using System.Reflection;
using Guppy.DependencyInjection.Interfaces;

namespace Guppy.ServiceLoaders
{
    [AutoLoad(Int32.MaxValue)]
    internal class ServiceLifecycleServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, GuppyServiceCollection services)
        {
            services.RegisterBuilder<IService>((s, p, sd) =>
            {
                s.OnStatusChanged += this.HandleServiceStatusChanged;
            }, Guppy.Core.Constants.Priorities.Create);

            services.RegisterBuilder<IService>((s, p, sd) => s.TryPreCreate(p) , Guppy.Core.Constants.Priorities.PreCreate);
            services.RegisterBuilder<IService>((s, p, sd) => s.TryCreate(p)    , Guppy.Core.Constants.Priorities.Create);
            services.RegisterBuilder<IService>((s, p, sd) => s.TryPostCreate(p), Guppy.Core.Constants.Priorities.PostCreate);

            services.RegisterSetup<IService>((s, p, sd) =>
            {
                s.ServiceConfiguration = sd;
            }, Int32.MinValue);

            services.RegisterSetup<IService>((s, p, sd) => s.TryPreInitialize(p) , Guppy.Core.Constants.Priorities.PreInitialize, this.SkipInitializationFilter);
            services.RegisterSetup<IService>((s, p, sd) => s.TryInitialize(p)    , Guppy.Core.Constants.Priorities.Initialize, this.SkipInitializationFilter);
            services.RegisterSetup<IService>((s, p, sd) => s.TryPostInitialize(p), Guppy.Core.Constants.Priorities.PostInitialize, this.SkipInitializationFilter);
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // 
        }

        #region Filters
        private bool SkipInitializationFilter(IAction<ServiceConfigurationKey, IServiceConfiguration> action, ServiceConfigurationKey key)
        {
            foreach (Type type in key.Type.GetInterfaces().Concat(key.Type))
                if (type.GetCustomAttribute<ManualInitializationAttribute>(inherit: true)?.Value ?? false)
                    return false;

            return true;
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// When an IService instance is marked not ready, we should automatically
        /// return the instance to the ServiceTypeDescriptor pool.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="old"></param>
        /// <param name="value"></param>
        private void HandleServiceStatusChanged(IService sender, ServiceStatus old, ServiceStatus value)
        {
            switch(value)
            {
                case ServiceStatus.NotInitialized:
                    if (old == ServiceStatus.PostReleasing)
                        if (!sender.ServiceConfiguration.TypeFactory.TryReturnToPool(sender))
                            sender.TryDispose();
                    break;
                case ServiceStatus.Disposing:
                    sender.OnStatusChanged -= this.HandleServiceStatusChanged;
                    break;
            }
        }
        #endregion
    }
}
