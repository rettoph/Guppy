using Guppy.Attributes;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent.DependencyInjection.Builders;
using Guppy.Interfaces;
using Guppy.Network.Components.Pipes;
using Guppy.Network.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.ServiceLoaders
{
    [AutoLoad]
    internal sealed class PipeServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, ServiceProviderBuilder services)
        {
            services.RegisterService<PipeService>()
                .SetLifetime(ServiceLifetime.Transient)
                .RegisterTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<PipeService>();
                });

            services.RegisterService<Pipe>()
                .SetLifetime(ServiceLifetime.Transient)
                .RegisterTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<Pipe>();
                });

            #region Register Components
            services.RegisterComponentService<PipeNetworkEntityRemoteComponent>()
                .RegisterTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<PipeNetworkEntityRemoteComponent>();
                })
                .RegisterComponentConfiguration(component =>
                {
                    component.SetAssignableEntityType<Pipe>();
                });
            #endregion
        }
    }
}
