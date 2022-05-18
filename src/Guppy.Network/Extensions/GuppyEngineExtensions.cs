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
        /// <returns></returns>
        public static GuppyEngine ConfigureNetwork(this GuppyEngine guppy, byte channelsCount)
        {
            if (guppy.Tags.Contains(nameof(ConfigureNetwork)))
            {
                return guppy;
            }

            return guppy.AddInitializer(new NetworkInitializer())
                .AddLoader(new LiteNetLibServiceLoader(channelsCount))
                .AddLoader(new NetworkServiceLoader())
                .AddLoader(new SettingsServiceLoader())
                .AddLoader(new SetupsServiceLoader())
                .AddLoader(new NetMessageServiceLoader())
                .AddLoader(new BusServiceLoader())
                .AddLoader(new SecurityLoader())
                .AddTag(nameof(ConfigureNetwork));
        }
    }
}
