using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Pong.Library;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pong.Server
{
    public class ServerPongGame : PongGame
    {
        public ServerPongGame(ILogger logger, IServiceProvider provider) : base(logger, provider)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
