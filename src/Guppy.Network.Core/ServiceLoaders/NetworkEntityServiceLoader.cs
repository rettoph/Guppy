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

            #region Register Components
            services.RegisterComponentService<NetworkEntityRemoteComponent>()
                .RegisterTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<NetworkEntityRemoteComponent>();
                })
                .RegisterComponentConfiguration(component =>
                {
                    component.SetAssignableEntityType<INetworkEntity>();
                });

            services.RegisterComponentService<NetworkEntityRemotePipeComponent>()
                .RegisterTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<NetworkEntityRemotePipeComponent>();
                })
                .RegisterComponentConfiguration(component =>
                {
                    component.SetAssignableEntityType<INetworkEntity>();
                });
            #endregion
        }

        public void ConfigureNetwork(NetworkProviderBuilder network)
        {
            network.RegisterDataType<CreateNetworkEntityMessage>()
                .SetReader(CreateNetworkEntityMessage.Read)
                .SetWriter(CreateNetworkEntityMessage.Write)
                .RegisterNetworkMessage(message =>
                {
                    message.SetDeliveryMethod(DeliveryMethod.ReliableOrdered)
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
                });  
        }
    }
}
