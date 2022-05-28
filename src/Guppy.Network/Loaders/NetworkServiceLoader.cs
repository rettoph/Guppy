using Guppy.Loaders;
using Guppy.Network.Peers;
using Guppy.Network.Providers;
using Microsoft.Extensions.DependencyInjection;
using Guppy.Network.Definitions.ComponentFilters;
using Guppy.Network.Services;
using Minnow.Providers;
using Guppy.Network;
using Guppy.Network.Utilities;
using Guppy.Network.Components;

namespace Guppy.Network.Loaders
{
    internal sealed class NetworkServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<INetScopeProvider, NetScopeProvider>();
            services.AddScoped<NetScope>();
            services.AddScoped<INetMessengerService, NetMessengerService>();
            services.AddScoped<Room>();
            services.AddScoped<RequestAuthorizer>();
            services.AddScoped<NetIdProvider>();

            services.AddActivated<Peer, ServerPeer>(singleton: true);
            services.AddActivated<Peer, ClientPeer>(singleton: true);

            services.AddComponentFilter<HostTypeRequiredComponentFilter>();
            services.AddComponentFilter<NetworkAuthorizationRequiredComponentFilter>();

            services.AddComponent<INetTarget, NetMessenger>();

            services.AddSingleton<ITypeProvider<INetTarget>>(p => p.GetRequiredService<IAssemblyProvider>().GetTypes<INetTarget>(t => t.IsConcrete()));
        }
    }
}
