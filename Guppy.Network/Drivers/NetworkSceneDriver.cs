using Guppy.Implementations;
using Lidgren.Network;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Drivers
{
    public class NetworkSceneDriver : Driver
    {
        protected NetworkScene scene { get; private set; }

        public NetworkSceneDriver(NetworkScene scene, IServiceProvider provider) : base(scene, provider)
        {
            this.scene = scene;
        }
    }
}
