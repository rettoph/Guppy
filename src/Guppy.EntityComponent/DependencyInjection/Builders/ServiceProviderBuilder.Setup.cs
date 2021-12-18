using Guppy.EntityComponent.Attributes;
using Guppy.EntityComponent.DependencyInjection.Builders.Interfaces;
using Guppy.EntityComponent.Enums;
using Guppy.EntityComponent.Interfaces;
using Guppy.EntityComponent.Utilities;
using Minnow.General.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.DependencyInjection.Builders
{
    public partial class ServiceProviderBuilder
    {
        private void SetupIEntityConfiguration()
        {
            this.RegisterService<ComponentManager>()
                .SetLifetime(ServiceLifetime.Transient)
                .SetTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<ComponentManager>();
                });

            this.RegisterBuilder<IEntity>()
                .SetMethod((e, p, c) =>
                {
                    e.OnStatusChanged += ServiceProviderBuilder.HandleEntityStatusChanged;
                });

            this.RegisterSetup<IEntity>()
                .SetOrder(Constants.Priorities.PreInitialize)
                .SetMethod((e, p, _) =>
                {
                    e.Components = p.GetService<ComponentManager>((manager, _, _) =>
                    {
                        IEnumerable<IComponent> components = CreateComponents(p.EntityComponentConfigurations[e.ServiceConfiguration.Id], e, p) ?? Enumerable.Empty<IComponent>();
                        manager.BuildDictionary(components);
                    });

                    e.Components.Do(component => component.TryPreInitialize(p));
                });

            this.RegisterSetup<IEntity>()
                .SetOrder(Constants.Priorities.Initialize)
                .SetMethod((e, p, _) =>
                {
                    e.Components.Do(component => component.TryInitialize(p));
                });

            this.RegisterSetup<IEntity>()
                .SetOrder(Constants.Priorities.PostInitialize)
                .SetMethod((e, p, _) =>
                {
                    e.Components.Do(component => component.TryPostInitialize(p));
                });
        }

        private void SetupIServiceConfiguration()
        {
            this.RegisterBuilder<IService>()
                .SetOrder(Constants.Priorities.Create)
                .SetMethod((s, p, sd) =>
                {
                    s.OnStatusChanged += ServiceProviderBuilder.HandleServiceStatusChanged;
                });

            this.RegisterBuilder<IService>()
                .SetOrder(Constants.Priorities.PreCreate)
                .SetMethod((s, p, sd) => s.TryPreCreate(p));

            this.RegisterBuilder<IService>()
                .SetOrder(Constants.Priorities.Create)
                .SetMethod((s, p, sd) => s.TryCreate(p));

            this.RegisterBuilder<IService>()
                .SetOrder(Constants.Priorities.PostCreate)
                .SetMethod((s, p, sd) => s.TryPostCreate(p));

            this.RegisterSetup<IService>()
                .SetOrder(Int32.MinValue)
                .SetMethod((s, p, sd) =>
                {
                    s.ServiceConfiguration = sd;
                });

            this.RegisterSetup<IService>().SetOrder(Constants.Priorities.PreInitialize).SetMethod((s, p, sd) => s.TryPreInitialize(p)).SetFilter(ServiceProviderBuilder.SkipInitializationFilter);
            this.RegisterSetup<IService>().SetOrder(Constants.Priorities.Initialize).SetMethod((s, p, sd) => s.TryInitialize(p)).SetFilter(ServiceProviderBuilder.SkipInitializationFilter);
            this.RegisterSetup<IService>().SetOrder(Constants.Priorities.PostInitialize).SetMethod((s, p, sd) => s.TryPostInitialize(p)).SetFilter(ServiceProviderBuilder.SkipInitializationFilter);
        }

        #region Helper Methods
        private static bool SkipInitializationFilter(IServiceConfigurationBuilder configuration)
        {
            Boolean CheckIsManualInitializationEnabled(Type type)
            {
                if (type is null)
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

        private static IEnumerable<IComponent> CreateComponents(
            ComponentConfiguration[] configurations,
            IEntity entity,
            ServiceProvider provider)
        {
            foreach (ComponentConfiguration configuration in configurations)
            {
                if (configuration.CheckFilters(entity, provider))
                {
                    IComponent component = configuration.ComponentServiceConfiguration.GetInstance(
                        provider: provider,
                        customSetup: (i, p, c) => (i as IComponent).Entity = entity,
                        customSetupOrder: Constants.Priorities.PreInitialize - 1
                    ) as IComponent;

                    yield return component;
                }
            }
        }
        #endregion

        #region Event Handlers
        private static void HandleEntityStatusChanged(IService sender, ServiceStatus old, ServiceStatus value)
        {
            if (sender is IEntity entity)
            {
                switch (value)
                {
                    case ServiceStatus.Releasing:
                        entity.Components.TryRelease();
                        break;
                    case ServiceStatus.Disposing:
                        entity.OnStatusChanged -= ServiceProviderBuilder.HandleEntityStatusChanged;
                        break;
                };
            }
        }

        /// <summary>
        /// When an IService instance is marked not ready, we should automatically
        /// return the instance to the ServiceTypeDescriptor pool.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="old"></param>
        /// <param name="value"></param>
        private static void HandleServiceStatusChanged(IService sender, ServiceStatus old, ServiceStatus value)
        {
            switch (value)
            {
                case ServiceStatus.NotInitialized:
                    if (old == ServiceStatus.PostReleasing)
                        if (!sender.ServiceConfiguration.TypeFactory.TryReturnToPool(sender))
                            sender.TryDispose();
                    break;
                case ServiceStatus.Disposing:
                    sender.OnStatusChanged -= ServiceProviderBuilder.HandleServiceStatusChanged;
                    break;
            }
        }
        #endregion
    }
}
