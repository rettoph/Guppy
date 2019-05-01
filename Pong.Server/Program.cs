using Guppy;
using Guppy.Loggers;
using Guppy.Network.Extensions.Guppy;
using Guppy.Network.Peers;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pong.Library.Scenes;
using Pong.Server.Scenes;
using Guppy.Network.Security;

namespace Pong.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            GameTime deltaGameTime;
            DateTime start = DateTime.Now;
            DateTime lastFrame = DateTime.Now;
            DateTime currentFrame = DateTime.Now;

            GuppyLoader guppy = new GuppyLoader(new ConsoleLogger());
            guppy.ConfigureNetwork(Program.PeerFactory);
            guppy.Initialize();

            Peer peer = guppy.Provider.GetRequiredService<Peer>();

            // Create the game
            ServerPongGame game = guppy.Games.Create<ServerPongGame>();
            game.SetScene(game.CreateScene<ServerLobbyScene>());

            while (true)
            {
                currentFrame = DateTime.Now;
                deltaGameTime = new GameTime(currentFrame.Subtract(start), currentFrame.Subtract(lastFrame));

                peer.Update();
                game.Update(deltaGameTime);

                Thread.Sleep(16);
            }
        }

        private static Peer PeerFactory(IServiceProvider arg)
        {
            var config = new NetPeerConfiguration("pong");
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.Error);
            config.ConnectionTimeout = 10;
            config.AutoFlushSendQueue = false;
            config.Port = 1337;

            return new ServerPeer(config, arg.GetService<ILogger>());
        }
    }
}
