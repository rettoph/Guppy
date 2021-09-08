using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Enums;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class EntityComponentServiceLoader : IServiceLoader
    {
        private static readonly IComponent[] EmptyComponentArray = new IComponent[0];

        public void RegisterServices(GuppyServiceCollection services)
        {
            services.RegisterTypeFactory<ComponentManager>(p => new ComponentManager());

            services.RegisterTransient<ComponentManager>();

            services.RegisterBuilder<IEntity>((e, p, c) =>
            {
                e.OnStatus[ServiceStatus.PostReleasing] += EntityComponentServiceLoader.HandleEntityPostReleasing;
                e.OnStatus[ServiceStatus.Disposing] += EntityComponentServiceLoader.HandleEntityDisposing;
            });

            services.RegisterSetup<IEntity>((e, p, _) =>
            {
                e.Components = p.GetService<ComponentManager>((manager, _, _) =>
                {
                    IEnumerable<IComponent> components = p.ComponentConfigurations[e.ServiceConfiguration.Key].Create(e, p) ?? Enumerable.Empty<IComponent>();
                    manager.BuildDictionary(components);
                });
            }, Guppy.Core.Constants.Priorities.PreInitialize - 1);
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }

        #region Event Handlers
        private static void HandleEntityPostReleasing(IService sender, ServiceStatus old, ServiceStatus value)
        {
            if (sender is IEntity entity)
            {
                entity.Components.TryRelease();
            }
        }

        private static void HandleEntityDisposing(IService sender, ServiceStatus old, ServiceStatus value)
        {
            sender.OnStatus[ServiceStatus.PreReleasing] -= EntityComponentServiceLoader.HandleEntityPostReleasing;
            sender.OnStatus[ServiceStatus.Disposing] -= EntityComponentServiceLoader.HandleEntityDisposing;
        }
        #endregion
    }
}
