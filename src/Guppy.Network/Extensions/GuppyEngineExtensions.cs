using Guppy.Network.Initializers;
using Guppy.Network.Loaders;
using Guppy.Network.Security.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    public static class GuppyEngineExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="guppy"></param>
        /// <param name="channelsCount">
        ///     <para><see cref="LiteNetLib.NetManager.ChannelsCount"/></para>
        ///     <para>QoS channel count per message type (value must be between 1 and 64 channels)</para>
        /// </param>
        /// <param name="autoStartNetMessengers">Automatically invoke <see cref="Network.Components.NetMessenger.Start(ushort)"/> if it is not done so after entity initialization.</param>
        /// <returns></returns>
        public static GuppyEngine ConfigureNetwork(this GuppyEngine guppy, byte channelsCount, bool autoStartNetMessengers)
        {
            if (guppy.Tags.Contains(nameof(ConfigureNetwork)))
            {
                return guppy;
            }

            return guppy.AddInitializer(new NetworkInitializer())
                .AddLoader(new LiteNetLibServiceLoader(channelsCount))
                .AddLoader(new NetworkServiceLoader())
                .AddLoader(new SettingsServiceLoader())
                .AddLoader(new SetupsServiceLoader(autoStartNetMessengers))
                .AddLoader(new NetMessageServiceLoader())
                .AddLoader(new BusServiceLoader())
                .AddLoader(new SecurityLoader())
                .AddTag(nameof(ConfigureNetwork));
        }
    }
}
