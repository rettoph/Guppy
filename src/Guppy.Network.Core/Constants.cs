﻿using Guppy.CommandLine;
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
            public const Int32 CreateNetworkEntityMessageQueue = Int32.MinValue + 1000;
            public const Int32 PingNetworkEntityMessageQueue = Int32.MinValue + 2000;
            public const Int32 RemoveNetworkEntityMessageQueue = Int32.MaxValue;
            public const Int32 DefaultMessageQueue = -1000;
        }
    }
}
