using Guppy.Network.Peers;
using Lidgren.Network;
using Pong.Library;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Guppy.Network.Security;
using Guppy.Network.Groups;
using Pong.Server.Scenes;
using Guppy.Extensions;

namespace Pong.Server
{
    public class ServerPongGame : PongGame
    {
        private ServerPeer _server;

        public ServerPongGame()
        {
        }

        protected override void Boot()
        {
            base.Boot();

            this.config.Port = 1337;

            this.services.AddScene<ServerLobbyScene>();
        }

        protected override void PostInitialize()
        {
            base.PostInitialize();

            _server = this.provider.GetService<ServerPeer>();
            _server.Users.Added += this.HandleUserJoinedServer;

            // Create a new lobby scene
            this.scenes.Create<ServerLobbyScene>();
        }

        protected override Peer PeerFactory(IServiceProvider arg)
        {
            return new ServerPeer(this.config, this.logger);
        }

        private void HandleUserJoinedServer(object sender, User e)
        {
            // When a new user joins, add them to the default group
            _server.Groups.GetOrCreateById(Guid.Empty).Users.Add(e);
        }
    }
}
