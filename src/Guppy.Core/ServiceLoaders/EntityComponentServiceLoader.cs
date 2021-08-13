using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Enums;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class EntityComponentServiceLoader : IServiceLoader
    {
        private static readonly IComponent[] EmptyComponentArray = new IComponent[0];

        public void RegisterServices(GuppyServiceCollection services)
        {
            services.RegisterBuilder<IEntity>((e, p, c) =>
            {
                e.OnStatus[ServiceStatus.PostReleasing] += EntityComponentServiceLoader.HandleEntityPostReleasing;
                e.OnStatus[ServiceStatus.Disposing] += EntityComponentServiceLoader.HandleEntityDisposing;
            });

            services.RegisterSetup<IEntity>((e, p, c) =>
            {
                e.Components = p.ComponentConfigurations[e.ServiceConfiguration.Key].Create(e, p) ?? EntityComponentServiceLoader.EmptyComponentArray;
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
                foreach (IComponent component in entity.Components)
                    component.TryRelease();

                entity.Components = EntityComponentServiceLoader.EmptyComponentArray;
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
