using Guppy.Network.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guppy.DependencyInjection;

namespace Guppy.Network.Lists
{
    public class ChannelList : BaseNetworkList<IChannel>
    {
        #region Create Methods
        /// <summary>
        /// Get or create a new <see cref="IChannel"/> with the given <paramref name="id"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IChannel GetOrCreate(Int16 id)
        {
            var channel = this.FirstOrDefault(c => c.Id == id);
            channel ??= this.Create<IChannel>(this.provider, (channel, p, c) =>
            {
                channel.Id = id;
            });

            return channel;
        }
        #endregion
    }
}
