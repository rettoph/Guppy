using Guppy.Extensions.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network
{
    public static class GuppyNetworkConstants
    {
        public static class MessageTypes
        {
            public static readonly UInt32 UserJoined = "user:joined".xxHash();
            public static readonly UInt32 UserLeft = "user:left".xxHash();
        }
    }
}
