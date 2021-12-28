using Guppy.Attributes;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent.DependencyInjection.Builders;
using Guppy.Interfaces;
using Guppy.Network.Builders;
using Guppy.Network.Components.NetworkEntities;
using Guppy.Network.Interfaces;
using Guppy.Network.MessageProcessors;
using Guppy.Network.Messages;
using Guppy.Network.Services;
using Guppy.Network.Utilities;
using Guppy.ServiceLoaders;
using Guppy.Threading.Utilities;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.ServiceLoaders
{
    [AutoLoad]
    internal sealed class NetworkEntityServiceLoader : IServiceLoader, INetworkLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, ServiceProviderBuilder services)
        {
            #region Register Services
            services.RegisterService<NetworkIdProvider>()
                .SetLifetime(ServiceLifetime.Scoped)
                .RegisterTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<NetworkIdProvider>();
                });

            services.RegisterService<NetworkEntityService>()
                .SetLifetime(ServiceLifetime.Scoped)
                .RegisterTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<NetworkEntityService>();
                });

            services.RegisterService<NetworkEntityPacketService>()
                .SetLifetime(ServiceLifetime.Transient)
                .RegisterTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<NetworkEntityPacketService>();
                });
            #endregion

            #region Entities
            services.RegisterEntity<INetworkEntity>()
                .RegisterComponent<NetworkEntityRemoteComponent>(component =>
                {
                    component.RegisterService(service =>
                    {
                        service.RegisterTypeFactory(factory =>
                        {
                            factory.SetDefaultConstructor<NetworkEntityRemoteComponent>();
                        });
                    });
                })
                .RegisterComponent<NetworkEntityRemotePipeComponent>(component =>
                {
                    component.RegisterService(service =>
                    {
                        service.RegisterTypeFactory(factory =>
                        {
                            factory.SetDefaultConstructor<NetworkEntityRemotePipeComponent>();
                        });
                    });
                });
            #endregion
        }

        public void ConfigureNetwork(NetworkProviderBuilder network)
        {
            network.RegisterNetworkEntityMessage<CreateNetworkEntityMessage>()
                .SetDeliveryMethod(DeliveryMethod.ReliableOrdered)
                .SetSequenceChannel(0)
                .SetFilter(CreateNetworkEntityMessage.Filter)
                .RegisterProcessorConfiguration<CreateNetworkEntityMessageProcessor>(service =>
                {
                    service.SetLifetime(ServiceLifetime.Scoped)
                        .RegisterTypeFactory(factory =>
                        {
                            factory.SetDefaultConstructor<CreateNetworkEntityMessageProcessor>();
                        });
                }); 
        }
    }
}
