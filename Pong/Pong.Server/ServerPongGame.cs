using Guppy.Attributes;
using Guppy.Network.Peers;
using Guppy.Network.Security.Authentication;
using Lidgren.Network;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Pong.Library;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pong.Server
{
    [IsGame]
    public class ServerPongGame : PongGame
    {
        private ServerPeer _server;

        public ServerPongGame(ServerPeer server) : base(server)
        {
            _server = server;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _server.Start();
            _server.Users.Events.AddDelegate<User>("added", this.HandleUserAdded);
        }

        #region Frame Methods
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.logger.LogDebug($"{_server.Users.Count}");
        }
        #endregion

        #region Event Handlers
        private void HandleUserAdded(object sender, User arg)
        {
            // Add the new user to the default group
            _server.Groups.GetOrCreateById(Guid.Empty).Users.Add(arg);
        }
        #endregion
    }
}
