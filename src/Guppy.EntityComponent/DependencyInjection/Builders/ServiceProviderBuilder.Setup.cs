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
                .RegisterTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<ComponentManager>();
                });

            this.RegisterSetup<IEntity>()
                .SetOrder(Constants.Priorities.PreInitialize - 1)
                .SetMethod((e, p, _) =>
                {
                    e.OnStatusChanged += ServiceProviderBuilder.HandleEntityStatusChanged;
                    e.Components = p.GetService<ComponentManager>((manager, _, _) =>
                    {
                        IEnumerable<IComponent> components = CreateComponents(p.EntityComponentConfigurations[e.ServiceConfiguration.Id], e, p) ?? Enumerable.Empty<IComponent>();
                        manager.BuildDictionary(components);
                    });
                });

            this.RegisterSetup<IEntity>()
                .SetOrder(Constants.Priorities.PreInitialize + 1)
                .SetMethod((e, p, _) =>
                {
                    e.Components.TryPreInitializeAll(p);
                });

            this.RegisterSetup<IEntity>()
                .SetOrder(Constants.Priorities.Initialize + 1)
                .SetMethod((e, p, _) =>
                {
                    e.Components.TryInitializeAll(p);
                });

            this.RegisterSetup<IEntity>()
                .SetOrder(Constants.Priorities.PostInitialize + 1)
                .SetMethod((e, p, _) =>
                {
                    e.Components.TryPostInitializeAll(p);
                });
        }

        private void SetupServiceConfiguration()
        {
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
        private static bool SkipInitializationFilter(ServiceConfigurationBuilder configuration)
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
                    case ServiceStatus.Uninitializing:
                        entity.Components.Dispose();
                        entity.OnStatusChanged -= ServiceProviderBuilder.HandleEntityStatusChanged;
                        break;
                };
            }
        }
        #endregion
    }
}
