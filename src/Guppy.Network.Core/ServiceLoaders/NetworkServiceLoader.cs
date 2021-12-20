using Guppy.EntityComponent.DependencyInjection;
using Guppy.Attributes;
using Guppy.Interfaces;
using Guppy.Network.Builders;
using Guppy.Network.Interfaces;
using Guppy.Network.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Security;
using Guppy.Network.Security.Enums;
using LiteNetLib.Utils;
using Guppy.Network.Security.Structs;
using LiteNetLib;
using Guppy.Network.Services;
using Guppy.Network.MessageProcessors;
using Guppy.Network.Enums;
using Guppy.Utilities;
using Guppy.EntityComponent.DependencyInjection.Builders;
using Guppy.EntityComponent.Utilities;
using Guppy.Threading.Utilities;

namespace Guppy.Network.ServiceLoaders
{
    [AutoLoad]
    internal sealed class NetworkServiceLoader : INetworkServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, ServiceProviderBuilder services)
        {
            services.RegisterService<RoomService>()
                .SetLifetime(ServiceLifetime.Singleton)
                .RegisterTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<RoomService>();
                });

            services.RegisterService<PipeService>()
                .SetLifetime(ServiceLifetime.Transient)
                .RegisterTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<PipeService>();
                });

            services.RegisterService<MessageQueue<IData>>()
                .SetLifetime(ServiceLifetime.Scoped)
                .RegisterTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<MessageQueue<IData>>();
                });

            services.RegisterService<NetworkMessageService>()
                .SetLifetime(ServiceLifetime.Scoped)
                .RegisterTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<NetworkMessageService>();
                });

            services.RegisterService<ClientPeer>()
                .SetLifetime(ServiceLifetime.Singleton)
                .AddCacheNamesBetweenTypes<Peer, ClientPeer>()
                .RegisterTypeFactory(builder =>
                {
                    builder.SetDefaultConstructor<ClientPeer>();
                });

            services.RegisterService<ServerPeer>()
                .SetLifetime(ServiceLifetime.Singleton)
                .AddCacheNamesBetweenTypes<Peer, ServerPeer>()
                .RegisterTypeFactory(builder =>
                {
                    builder.SetDefaultConstructor<ServerPeer>();
                });

            services.RegisterSetup<Settings>()
                .SetMethod((s, _, _) => s.Set(HostType.Local));
        }

        public void ConfigureNetwork(NetworkProviderBuilder network)
        {
            network.RegisterDataType<ConnectionRequestMessage>()
                .SetReader(ConnectionRequestMessage.Read)
                .SetWriter(ConnectionRequestMessage.Write);

            network.RegisterDataType<ConnectionRequestResponseMessage>()
                .SetReader(ConnectionRequestResponseMessage.Read)
                .SetWriter(ConnectionRequestResponseMessage.Write)
                .RegisterNetworkMessage(message =>
                {
                    message.SetDeliveryMethod(DeliveryMethod.ReliableOrdered)
                        .SetSequenceChannel(0)
                        .SetPeerFilter<ClientPeer>()
                        .RegisterProcessorConfiguration<ConnectionRequestResponseMessageProcessor>(processor =>
                        {
                            processor.SetLifetime(ServiceLifetime.Singleton)
                                .RegisterTypeFactory(factory =>
                                {
                                    factory.SetDefaultConstructor<ConnectionRequestResponseMessageProcessor>();
                                });
                        });
                });
        }
    }
}
