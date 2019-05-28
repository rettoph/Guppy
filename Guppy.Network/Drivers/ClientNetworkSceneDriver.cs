using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Guppy.Network.Drivers
{
    public class ClientNetworkSceneDriver : NetworkSceneDriver
    {
        public ClientNetworkSceneDriver(NetworkScene scene, IServiceProvider provider, ILogger logger) : base(scene, provider, logger)
        {
        }
    }
}
