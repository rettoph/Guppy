using Guppy.CommandLine;
using Guppy.Threading.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public static class Constants
    {
        public static class Queues
        {
            public const Int32 UserRoomActionMessageQueue = Int32.MinValue;
            public const Int32 CreateNetworkEntityIncomingMessagePriority  = 1000;
            public const Int32 DefaultIncomingMessagePriority              = 1010;
            public const Int32 DisposeNetworkEntityIncomingMessagePriority = 1020;

            public const Int32 DefaultOutgoingMessagePriority              = 1000;
            public const Int32 DisposeNetworkEntityOutgoingMessagePriority = 1010;
            public const Int32 CreateNetworkEntityOutgoingMessagePriority  = 1010;
        }
    }
}
