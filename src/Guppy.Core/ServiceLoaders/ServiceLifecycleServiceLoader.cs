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
using System.Reflection;
using Guppy.DependencyInjection.Builders;
using DotNetUtils.General.Interfaces;
using DotNetUtils.DependencyInjection;
using DotNetUtils.DependencyInjection.Builders.Interfaces;

namespace Guppy.ServiceLoaders
{
    [AutoLoad(Int32.MaxValue)]
    internal class ServiceLifecycleServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, GuppyServiceProviderBuilder services)
        {
            services.RegisterBuilder<IService>()
                .SetOrder(Guppy.Core.Constants.Priorities.Create)
                .SetMethod((s, p, sd) =>
                {
                    s.OnStatusChanged += this.HandleServiceStatusChanged;
                });

            services.RegisterBuilder<IService>()
                .SetOrder(Guppy.Core.Constants.Priorities.PreCreate)
                .SetMethod((s, p, sd) => s.TryPreCreate(p));

            services.RegisterBuilder<IService>()
                .SetOrder(Guppy.Core.Constants.Priorities.Create)
                .SetMethod((s, p, sd) => s.TryCreate(p));

            services.RegisterBuilder<IService>()
                .SetOrder(Guppy.Core.Constants.Priorities.PostCreate)
                .SetMethod((s, p, sd) => s.TryPostCreate(p));

            services.RegisterSetup<IService>()
                .SetOrder(Int32.MinValue)
                .SetMethod((s, p, sd) =>
                {
                    s.ServiceConfiguration = sd;
                });

            services.RegisterSetup<IService>().SetOrder(Guppy.Core.Constants.Priorities.PreInitialize).SetMethod((s, p, sd) => s.TryPreInitialize(p)).SetFilter(this.SkipInitializationFilter);
            services.RegisterSetup<IService>().SetOrder(Guppy.Core.Constants.Priorities.Initialize).SetMethod((s, p, sd) => s.TryInitialize(p)).SetFilter(this.SkipInitializationFilter);
            services.RegisterSetup<IService>().SetOrder(Guppy.Core.Constants.Priorities.PostInitialize).SetMethod((s, p, sd) => s.TryPostInitialize(p)).SetFilter(this.SkipInitializationFilter);
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // 
        }

        #region Filters
        private bool SkipInitializationFilter(IServiceConfigurationBuilder<GuppyServiceProvider> configuration)
        {
            Boolean CheckIsManualInitializationEnabled(Type type)
            {
                if(type is null)
                {
                    throw new ArgumentOutOfRangeException(nameof(type));
                }

                return type.GetCustomAttribute<ManualInitializationAttribute>(inherit: true)?.Value ?? false;
            }

            if (CheckIsManualInitializationEnabled(configuration.FactoryType))
            {
                return true;
            }

            foreach (Type type in configuration.FactoryType.GetInterfaces())
                if (CheckIsManualInitializationEnabled(type))
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
