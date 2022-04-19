using Guppy.Loaders;
using Guppy.Network.Components;
using Guppy.Network.Peers;
using Guppy.Network.Providers;
using Microsoft.Extensions.DependencyInjection;
using Guppy.Network.Definitions.ComponentFilters;

namespace Guppy.Network.Loaders
{
    internal sealed class NetworkServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IRoomProvider, RoomProvider>();

            services.AddActivated<Peer, ServerPeer>();
            services.AddActivated<Peer, ClientPeer>();

            services.AddComponentFilter<HostTypeRequiredComponentFilter>();
            services.AddComponentFilter<NetworkAuthorizationRequiredComponentFilter>();

            services.AddComponent<Room, RoomRemoteMasterComponent>();
            services.AddComponent<Room, RoomRemoteSlaveComponent>();
        }
    }
}
