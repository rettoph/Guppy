using Guppy.Extensions.System;
using Guppy.Network.Contexts;
using Guppy.Network.Interfaces;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Extensions
{
    public static class IChannelExtensions
    {
        public static void SignMessage(this IChannel channel, NetOutgoingMessage om)
            => om.Write(channel.Id);
    }
}
