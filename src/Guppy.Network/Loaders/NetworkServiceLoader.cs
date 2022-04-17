using Guppy.EntityComponent;
using Guppy.EntityComponent.Loaders;
using Guppy.EntityComponent.Loaders.Collections;
using Guppy.EntityComponent.Providers;
using Guppy.Loaders;
using Guppy.Network.Attributes;
using Guppy.Network.Components;
using Guppy.Network.Constants;
using Guppy.Network.Enums;
using Guppy.Network.Loaders.Definitions.ComponentFilters;
using Guppy.Network.Loaders.Definitions.Settings;
using Guppy.Network.Loaders.Definitions.SettingSerializers;
using Guppy.Network.Peers;
using Guppy.Network.Providers;
using Guppy.Network.Security.Messages;
using Guppy.Settings.Loaders;
using Guppy.Settings.Loaders.Collections;
using Guppy.Settings.Providers;
using Guppy.Threading;
using Guppy.Threading.Loaders;
using Microsoft.Extensions.DependencyInjection;
using Minnow.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Loaders
{
    internal sealed class NetworkServiceLoader : IServiceLoader, IComponentLoader, ISettingLoader
    {
        public void ConfigureSettings(ISettingCollection settings, ISettingSerializerCollection serializers)
        {
            serializers.Add<HostTypeSettingSerializer>();
            serializers.Add<NetworkAuthorizationSettingSerializer>();


            settings.Add<HostTypeSetting>();
            settings.Add<NetworkAuthorizationSetting>();
        }

        public void ConfigureComponents(IComponentCollection components, IComponentFilterCollection filters)
        {
            filters.Add<HostTypeRequiredComponentFilter>();
            filters.Add<NetworkAuthorizationRequiredComponentFilter>();

            components.Add<Room, RoomRemoteHostMasterComponent>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IRoomProvider, RoomProvider>();

            services.AddActivated<Peer, ServerPeer>();
            services.AddActivated<Peer, ClientPeer>();
        }
    }
}
