using Guppy.Collections;
using Guppy.Interfaces;
using Guppy.Network.Collections;
using Guppy.Network.Peers;
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
            services.AddScoped<NetworkEntityCollection>(p =>
            {
                return new NetworkEntityCollection(p.GetService<EntityCollection>());
            });
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
