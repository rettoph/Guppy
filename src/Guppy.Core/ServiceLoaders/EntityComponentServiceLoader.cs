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
                e.OnStatusChanged += this.HandleEntityStatusChanged;
            });

            services.RegisterSetup<IEntity>((e, p, _) =>
            {
                e.Components = p.GetService<ComponentManager>((manager, _, _) =>
                {
                    IEnumerable<IComponent> components = p.ComponentConfigurations[e.ServiceConfiguration.Key].Create(e, p) ?? Enumerable.Empty<IComponent>();
                    manager.BuildDictionary(components);
                });

                e.Components.Do(component => component.TryPreInitialize(p));
            }, Guppy.Core.Constants.Priorities.PreInitialize);

            services.RegisterSetup<IEntity>((e, p, _) =>
            {
                e.Components.Do(component => component.TryInitialize(p));
            }, Guppy.Core.Constants.Priorities.Initialize);

            services.RegisterSetup<IEntity>((e, p, _) =>
            {
                e.Components.Do(component => component.TryPostInitialize(p));
            }, Guppy.Core.Constants.Priorities.PostInitialize);
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }

        #region Event Handlers
        private void HandleEntityStatusChanged(IService sender, ServiceStatus old, ServiceStatus value)
        {
            if(sender is IEntity entity)
            {
                switch (value)
                {
                    case ServiceStatus.PreReleasing:
                        entity.Components.TryRelease();
                        break;
                    case ServiceStatus.Disposing:
                        entity.OnStatusChanged -= this.HandleEntityStatusChanged;
                        break;
                };
            }
        }
        #endregion
    }
}
