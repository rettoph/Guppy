using DotNetUtils.DependencyInjection;
using DotNetUtils.General.Interfaces;
using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.DependencyInjection.Builders;
using Guppy.Enums;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class EntityComponentServiceLoader : IServiceLoader
    {
        private static readonly IComponent[] EmptyComponentArray = new IComponent[0];

        public void RegisterServices(AssemblyHelper assemblyHelper, GuppyServiceProviderBuilder services)
        {
            services.RegisterService<ComponentManager>()
                .SetLifetime(ServiceLifetime.Transient)
                .SetTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<ComponentManager>();
                });

            services.RegisterBuilder<IEntity>()
                .SetMethod((e, p, c) =>
                {
                    e.OnStatusChanged += this.HandleEntityStatusChanged;
                });

            services.RegisterSetup<IEntity>()
                .SetOrder(Guppy.Core.Constants.Priorities.PreInitialize)
                .SetMethod((e, p, _) =>
                {
                    e.Components = p.GetService<ComponentManager>((manager, _, _) =>
                    {
                        IEnumerable<IComponent> components = p.EntityComponentConfigurations[e.ServiceConfiguration.Id].Create(e, p) ?? Enumerable.Empty<IComponent>();
                        manager.BuildDictionary(components);
                    });
                    
                    e.Components.Do(component => component.TryPreInitialize(p));
                });

            services.RegisterSetup<IEntity>()
                .SetOrder(Guppy.Core.Constants.Priorities.Initialize)
                .SetMethod((e, p, _) =>
                {
                    e.Components.Do(component => component.TryInitialize(p));
                });

            services.RegisterSetup<IEntity>()
                .SetOrder(Guppy.Core.Constants.Priorities.PostInitialize)
                .SetMethod((e, p, _) =>
                {
                    e.Components.Do(component => component.TryPostInitialize(p));
                });
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
                    case ServiceStatus.Releasing:
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
