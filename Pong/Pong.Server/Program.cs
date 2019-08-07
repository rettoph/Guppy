using Guppy;
using Guppy.Network.Extensions;
using Lidgren.Network;
using System;

namespace Pong.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            // Configure guppy...
            var guppy = new GuppyLoader();
            var config = new NetPeerConfiguration("pong")
            {
                Port = 1337
            };

            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            guppy.ConfigureServer(config);
            guppy.Initialize();

            // Build the game instance...
            var game = guppy.BuildGame<ServerPongGame>();

            // Start the game...
            game.TryStartAsync();

            Console.ReadLine();

            game.TryStopAsync();
            game.Dispose();

            Console.ReadLine();
        }
    }
}
