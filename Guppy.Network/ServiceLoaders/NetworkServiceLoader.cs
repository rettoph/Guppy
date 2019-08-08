﻿using Guppy.Attributes;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Network.Configurations;
using Guppy.Network.Peers;
using Guppy.Utilities.Pools;
using Lidgren.Network;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Network.Security.Authentication;
using Guppy.Utilities.Loaders;
using Guppy.Network.Extensions.DependencyInjection;
using Guppy.Network.Security.Authentication.Authenticators;
using Guppy.Network.Collections;
using Guppy.Network.Groups;

namespace Guppy.Network.ServiceLoaders
{
    /// <summary>
    /// Default service loader that includes
    /// core network services.
    /// </summary>
    [IsServiceLoader()]
    public class NetworkServiceLoader : IServiceLoader
    {
        public void Boot(IServiceCollection services)
        {
            services.AddAuthenticator<BasicAuthenticator>();
            services.TryAddPool<Claim, ServicePool<Claim>>();
            services.AddTransient<UserCollection>();

            services.TryAddPool<Group, InitializablePool<Group>>();
            services.TryAddPool<NetOutgoingMessageConfiguration, ServicePool<NetOutgoingMessageConfiguration>>();
            services.AddScoped<NetPeer>(p => p.GetConfigurationValue<NetPeer>("net-peer"));
            services.AddScoped<Peer>(p => p.GetConfigurationValue<Peer>("peer"));
            services.TryAddPool<GroupCollection, ServicePool<GroupCollection>>();
            services.AddScoped<GroupCollection>(p => p.GetConfigurationValueOrCreate<GroupCollection>("group-collection", g =>
            {
                p.SetConfigurationValue("group-collection", g);
            }));
        }

        public void PreInitialize(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }

        public void Initialize(IServiceProvider provider)
        {
            var entityLoader = provider.GetLoader<EntityLoader>();
            entityLoader.TryRegister<User>("guppy:network:entity:user");
        }

        public void PostInitialize(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
