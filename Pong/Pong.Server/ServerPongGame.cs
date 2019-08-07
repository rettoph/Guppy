using Guppy.Attributes;
using Guppy.Network.Peers;
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

            _server.Events.AddDelegate<NetIncomingMessage>("recieved:connection-approval", this.HandleConnectionApprovalMessage);
        }

        #region Frame Methods
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        #endregion

        #region Event Handlers
        private void HandleConnectionApprovalMessage(object sender, NetIncomingMessage arg)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
