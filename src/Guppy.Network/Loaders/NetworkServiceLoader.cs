using Guppy.EntityComponent;
using Guppy.EntityComponent.Loaders;
using Guppy.EntityComponent.Providers;
using Guppy.Loaders;
using Guppy.Network.Attributes;
using Guppy.Network.Components;
using Guppy.Network.Constants;
using Guppy.Network.Definitions.SettingSerializers;
using Guppy.Network.Enums;
using Guppy.Network.Definitions.Settings;
using Guppy.Network.Peers;
using Guppy.Network.Providers;
using Guppy.Network.Security.Messages;
using Guppy.Settings.Providers;
using Guppy.Threading;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            services.AddSettingSerializer<HostTypeSettingSerializer>();
            services.AddSettingSerializer<NetworkAuthorizationSettingSerializer>();


            services.AddSetting<HostTypeSetting>();
            services.AddSetting<NetworkAuthorizationSetting>();

            services.AddComponentFilter<HostTypeRequiredComponentFilter>();
            services.AddComponentFilter<NetworkAuthorizationRequiredComponentFilter>();

            services.AddComponent<Room, RoomRemoteHostMasterComponent>();
        }
    }
}
