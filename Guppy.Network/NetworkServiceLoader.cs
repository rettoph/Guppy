using Guppy.Collections;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Network.Collections;
using Guppy.Network.Factories;
using Guppy.Network.Peers;
using Guppy.Network.Utilities.DynamicDelegaters;
using Lidgren.Network;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network
{
    public class NetworkServiceLoader : IServiceLoader
    {
        public void ConfigureServiceCollection(IServiceCollection services)
        {
            services.AddSingleton<NetOutgoingMessageConfigurationPool>();
            services.AddSingleton<GlobalUserCollection>();
            services.AddSingleton<GroupCollection>();
            services.AddSingleton<GroupFactory>();
            services.AddSingleton<NetPeer>(p => p.GetRequiredService<Peer>().GetNetPeer());
            services.AddScoped<NetworkEntityCollection>(p =>
            {
                return new NetworkEntityCollection(p.GetService<EntityCollection>());
            });
            services.AddTransient<MessageDelegater>();


            services.AddScene<NetworkScene>();
        }

        public void Boot(IServiceProvider provider)
        {
            var peer = provider.GetRequiredService<Peer>();
            peer.Start();
        }

        public void PreInitialize(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }

        public void Initialize(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }

        public void PostInitialize(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
