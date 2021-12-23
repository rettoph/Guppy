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
using Guppy.Network.Components;
using Minnow.General;
using Guppy.Network.Attributes;
using System.Reflection;
using Guppy.Network.Components.Rooms;
using Guppy.Network.Utilities;
using Guppy.Network.Components.NetworkEntities;

namespace Guppy.Network.ServiceLoaders
{
    [AutoLoad]
    internal sealed class NetworkServiceLoader : IServiceLoader, INetworkLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, ServiceProviderBuilder services)
        {
            #region Register Services
            services.RegisterService<RoomService>()
                .SetLifetime(ServiceLifetime.Singleton)
                .RegisterTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<RoomService>();
                });

            services.RegisterService<Room>()
                .SetLifetime(ServiceLifetime.Transient)
                .RegisterTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<Room>();
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

            services.RegisterService<AttributeCache<NetworkAuthorizationRequiredAttribute>>()
                .SetLifetime(ServiceLifetime.Singleton)
                .RegisterTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<AttributeCache<NetworkAuthorizationRequiredAttribute>>();
                });

            services.RegisterService<AttributeCache<NetworkAuthorizationRequiredAttribute>>()
                .SetLifetime(ServiceLifetime.Singleton)
                .RegisterTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<AttributeCache<NetworkAuthorizationRequiredAttribute>>();
                });

            services.RegisterService<AttributeCache<HostTypeRequiredAttribute>>()
                .SetLifetime(ServiceLifetime.Singleton)
                .RegisterTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<AttributeCache<HostTypeRequiredAttribute>>();
                });
            #endregion

            #region Register Component Filters
            services.RegisterComponentFilter(typeof(NetworkComponent<>))
                .SetMethod((e, p, c) =>
                {
                    NetworkAuthorization requiredNetworkAuthorization = p.GetService<AttributeCache<NetworkAuthorizationRequiredAttribute>>()[c.TypeFactory.Type].NetworkAuthorization;
                    return requiredNetworkAuthorization == p.Settings.Get<NetworkAuthorization>();
                })
                .SetFilter(cc =>
                {
                    var hasAttribute = cc.TypeFactory.Type.GetCustomAttribute<NetworkAuthorizationRequiredAttribute>() != default;
                    return hasAttribute;
                });

            services.RegisterComponentFilter(typeof(NetworkComponent<>))
                .SetMethod((e, p, c) =>
                {
                    HostType requiredNetworkAuthorization = p.GetService<AttributeCache<HostTypeRequiredAttribute>>()[c.TypeFactory.Type].HostType;
                    return requiredNetworkAuthorization == p.Settings.Get<HostType>();
                })
                .SetFilter(cc =>
                {
                    var hasAttribute = cc.TypeFactory.Type.GetCustomAttribute<HostTypeRequiredAttribute>() != default;
                    return hasAttribute;
                });
            #endregion

            #region Register Components
            services.RegisterComponentService<RoomRemoteMasterComponent>()
                .RegisterTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<RoomRemoteMasterComponent>();
                })
                .RegisterComponentConfiguration(component =>
                {
                    component.SetAssignableEntityType<Room>();
                });
            #endregion
        }

        public void ConfigureNetwork(NetworkProviderBuilder network)
        {
            network.RegisterDataType<ConnectionRequestMessage>()
                .SetReader(ConnectionRequestMessage.Read)
                .SetWriter(ConnectionRequestMessage.Write)
                .RegisterNetworkMessage(message =>
                {
                    // This message is never sent traditionally
                    // So theres no need to build a processor for it
                    message.SetFilter((_, _) => false);
                });

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

            network.RegisterDataType<UserRoomActionMessage>()
                .SetReader(UserRoomActionMessage.Read)
                .SetWriter(UserRoomActionMessage.Write)
                .RegisterNetworkMessage(message =>
                {
                    message.SetDeliveryMethod(DeliveryMethod.ReliableOrdered)
                        .SetSequenceChannel(0)
                        .SetPeerFilter<ClientPeer>()
                        .SetProcessorConfiguration<ClientPeer>();
                });
        }
    }
}
