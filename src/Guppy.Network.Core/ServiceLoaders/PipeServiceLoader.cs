using Guppy.Attributes;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent.DependencyInjection.Builders;
using Guppy.Interfaces;
using Guppy.Network.Components.Pipes;
using Guppy.Network.Services;
using Guppy.ServiceLoaders;
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

            services.RegisterEntity<Pipe>()
                .RegisterService(service =>
                {
                    service.SetLifetime(ServiceLifetime.Transient)
                        .RegisterTypeFactory(factory =>
                        {
                            factory.SetDefaultConstructor<Pipe>();
                        });
                })
                .RegisterComponent<PipeNetworkEntityRemoteComponent>(component =>
                {
                    component.RegisterService(service =>
                    {
                        service.RegisterTypeFactory(factory =>
                        {
                            factory.SetDefaultConstructor<PipeNetworkEntityRemoteComponent>();
                        });
                    });
                });
        }
    }
}
