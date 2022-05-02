using Guppy.Loaders;
using LiteNetLib;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Loaders
{
    internal sealed class LiteNetLibServiceLoader : IServiceLoader
    {
        /// <summary>
        /// QoS channel count per message type (value must be between 1 and 64 channels)
        /// </summary>
        private readonly byte _channelsCount;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelsCount">QoS channel count per message type (value must be between 1 and 64 channels)</param>
        public LiteNetLibServiceLoader(byte channelsCount)
        {
            _channelsCount = channelsCount;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<EventBasedNetListener>();
            services.AddSingleton<NetManager>(p =>
            {
                var listener = p.GetRequiredService<EventBasedNetListener>();
                var manager = new NetManager(listener)
                {
                    ChannelsCount = _channelsCount,
                };

                return manager;
            });
        }
    }
}
