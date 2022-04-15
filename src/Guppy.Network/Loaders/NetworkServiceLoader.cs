using Guppy.EntityComponent.Loaders;
using Guppy.EntityComponent.Loaders.Collections;
using Guppy.EntityComponent.Providers;
using Guppy.Loaders;
using Guppy.Network.Constants;
using Guppy.Network.Enums;
using Guppy.Network.Peers;
using Guppy.Network.Providers;
using Guppy.Network.Security.Messages;
using Guppy.Threading;
using Guppy.Threading.Loaders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Loaders
{
    internal sealed class NetworkServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IRoomProvider, RoomProvider>();

            services.AddActivated<Peer, ServerPeer>();
            services.AddActivated<Peer, ClientPeer>();
        }
    }
}
