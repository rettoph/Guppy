using Guppy.Network;
using Pong.Server.Configurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pong.Server.Scenes
{
    public class ServerGameScene : NetworkScene
    {
        public ServerGameScene(ServerGameSceneConfiguration config, IServiceProvider provider) : base(provider)
        {
        }
    }
}
