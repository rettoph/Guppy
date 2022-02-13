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
using Guppy.ServiceLoaders;
using Guppy.EntityComponent;
using Guppy.EntityComponent.Interfaces;

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

            services.RegisterEntity<Room>()
                .RegisterService(service =>
                {
                    service.SetLifetime(ServiceLifetime.Transient)
                        .RegisterTypeFactory(factory =>
                        {
                            factory.SetDefaultConstructor<Room>();
                        });
                })
                .RegisterComponent<RoomRemoteMasterComponent>(component =>
                {
                    component.RegisterService(service =>
                    {
                        service.RegisterTypeFactory(factory =>
                        {
                            factory.SetDefaultConstructor<RoomRemoteMasterComponent>();
                        });
                    });
                });

            services.RegisterService<ClientPeer>()
                .SetLifetime(ServiceLifetime.Singleton)
                .AddAllAliases<Peer>()
                .RegisterTypeFactory(builder =>
                {
                    builder.SetDefaultConstructor<ClientPeer>();
                });

            services.RegisterService<ServerPeer>()
                .SetLifetime(ServiceLifetime.Singleton)
                .AddAllAliases<Peer>()
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
            services.RegisterComponentFilter(typeof(IComponent))
                .SetMethod((e, p, c) =>
                {
                    var cache = p.GetService<AttributeCache<NetworkAuthorizationRequiredAttribute>>();
                    NetworkAuthorization requiredNetworkAuthorization = cache[c.TypeFactory.Type].NetworkAuthorization;
                    var result = (p.Settings.Get<NetworkAuthorization>() & requiredNetworkAuthorization) != 0;

                    return result;
                })
                .SetFilter(cc =>
                {
                    return cc.TypeFactory.Type.HasCustomAttribute<NetworkAuthorizationRequiredAttribute>();
                });

            services.RegisterComponentFilter(typeof(IComponent))
                .SetMethod((e, p, c) =>
                {
                    HostType requiredNetworkAuthorization = p.GetService<AttributeCache<HostTypeRequiredAttribute>>()[c.TypeFactory.Type].HostType;
                    return requiredNetworkAuthorization == p.Settings.Get<HostType>();
                })
                .SetFilter(cc =>
                {
                    return cc.TypeFactory.Type.HasCustomAttribute<HostTypeRequiredAttribute>();
                });
            #endregion
        }

        public void ConfigureNetwork(NetworkProviderBuilder network)
        {
            // This message is never sent traditionally
            // So theres no need to build a processor for it
            network.RegisterNetworkMessage<ConnectionRequestMessage>()
                .SetSenderAuthorization(NetworkAuthorization.Master)
                .RegisterDataType(dataType =>
                {
                    dataType
                        .SetReader(ConnectionRequestMessage.Read)
                        .SetWriter(ConnectionRequestMessage.Write);
                });

            network.RegisterNetworkMessage<ConnectionRequestResponseMessage>()
                .SetDeliveryMethod(DeliveryMethod.ReliableOrdered)
                .SetSequenceChannel(0)
                .SetSenderAuthorization(NetworkAuthorization.Master)
                .RegisterProcessorConfiguration<ConnectionRequestResponseMessageProcessor>(processor =>
                {
                    processor.SetLifetime(ServiceLifetime.Singleton)
                        .RegisterTypeFactory(factory =>
                        {
                            factory.SetDefaultConstructor<ConnectionRequestResponseMessageProcessor>();
                        });
                })
                .RegisterDataType(dataType =>
                {
                    dataType
                        .SetReader(ConnectionRequestResponseMessage.Read)
                        .SetWriter(ConnectionRequestResponseMessage.Write);
                });

            network.RegisterNetworkMessage<UserRoomActionMessage>()
                .SetSequenceChannel(0)
                .SetSenderAuthorization(NetworkAuthorization.Master)
                .SetProcessorConfiguration<ClientPeer>()
                .SetIncomingPriority(Constants.Queues.UserRoomActionMessageQueue)
                .RegisterDataType(dataType =>
                {
                    dataType
                        .SetReader(UserRoomActionMessage.Read)
                        .SetWriter(UserRoomActionMessage.Write);
                });
        }
    }
}
