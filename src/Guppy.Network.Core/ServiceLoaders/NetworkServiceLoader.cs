using Guppy.EntityComponent.DependencyInjection;
using Guppy.Attributes;
using Guppy.Interfaces;
using Guppy.Network.Builders;
using Guppy.Network.Interfaces;
using Guppy.Network.Dtos;
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

namespace Guppy.Network.ServiceLoaders
{
    [AutoLoad]
    internal sealed class NetworkServiceLoader : INetworkServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, ServiceProviderBuilder services)
        {
            services.RegisterService<RoomService>()
                .SetLifetime(ServiceLifetime.Singleton)
                .SetTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<RoomService>();
                });

            services.RegisterService<PipeService>()
                .SetLifetime(ServiceLifetime.Transient)
                .SetTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<PipeService>();
                });

            services.RegisterService<MessageService>()
                .SetLifetime(ServiceLifetime.Scoped)
                .SetTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<MessageService>();
                });

            services.RegisterService<ClientPeer>()
                .SetLifetime(ServiceLifetime.Singleton)
                .AddCacheNamesBetweenTypes<Peer, ClientPeer>()
                .SetTypeFactory(builder =>
                {
                    builder.SetDefaultConstructor<ClientPeer>();
                });

            services.RegisterService<ServerPeer>()
                .SetLifetime(ServiceLifetime.Singleton)
                .AddCacheNamesBetweenTypes<Peer, ServerPeer>()
                .SetTypeFactory(builder =>
                {
                    builder.SetDefaultConstructor<ServerPeer>();
                });

            services.RegisterSetup<Settings>()
                .SetMethod((s, _, _) => s.Set(HostType.Local));
        }

        public void ConfigureNetwork(NetworkProviderBuilder network)
        {
            network.RegisterDataType<ConnectionRequestDto>()
                .SetReader(ConnectionRequestDto.Read)
                .SetWriter(ConnectionRequestDto.Write);

            network.RegisterDataType<ConnectionRequestResponseDto>()
                .SetReader(ConnectionRequestResponseDto.Read)
                .SetWriter(ConnectionRequestResponseDto.Write)
                .RegisterMessage(Constants.Messages.ConnectionRequestResponse, message =>
                {
                    message.SetDeliveryMethod(DeliveryMethod.ReliableOrdered)
                        .SetSequenceChannel(0)
                        .SetPeerFilter<ClientPeer>()
                        .SetProcessorFactory(ConnectionRequestResponseMessageProcessor.Factory);
                });
        }
    }
}
