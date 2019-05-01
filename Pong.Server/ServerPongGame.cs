using Guppy.Network.Peers;
using Guppy.Network.Security;
using Lidgren.Network;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Pong.Library;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Network.Groups;
using System.Threading;

namespace Pong.Server
{
    public class ServerPongGame : PongGame
    {
        public Boolean Running { get; set; }
        private Thread _loop;


        public ServerPongGame(ServerPeer server, ILogger logger, IServiceProvider provider) : base(logger, provider)
        {
        }

        public void RunAsync()
        {
            this.Running = true;

            _loop = new Thread(new ThreadStart(this.Loop));
        }

        private void Loop()
        {
            GameTime deltaGameTime;
            DateTime start = DateTime.Now;
            DateTime lastFrame = DateTime.Now;
            DateTime currentFrame = DateTime.Now;

            while (this.Running)
            {
                currentFrame = DateTime.Now;
                deltaGameTime = new GameTime(currentFrame.Subtract(start), currentFrame.Subtract(lastFrame));

                this.Update(deltaGameTime);

                Thread.Sleep(16);
            }

            // Auto dispose of the game after run complete
            this.Dispose();
        }
    }
}
