using Guppy;
using Guppy.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pong.Client.Scenes
{
    public class ClientGameScene : NetworkScene
    {
        public ClientGameScene(IServiceProvider provider) : base(provider)
        {
        }
    }
}
