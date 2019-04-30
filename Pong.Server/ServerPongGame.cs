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

namespace Pong.Server
{
    public class ServerPongGame : PongGame
    {
        private ServerPeer _server;

        public ServerPongGame(ServerPeer server, ILogger logger, IServiceProvider provider) : base(logger, provider)
        {
            _server = server;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.Group.MessageHandler.Add("chat", this.HandleChatMessage);

            _server.OnUserConnected += this.HandleUserConnected;
        }

        private void HandleChatMessage(NetIncomingMessage obj)
        {
            var content = obj.ReadString();

            var om = this.Group.CreateMessage("chat");
            om.Write(this.Group.Users.GetByNetConnection(obj.SenderConnection).Id);
            om.Write(content);

            this.Group.SendMesssage(om, NetDeliveryMethod.ReliableOrdered);
        }

        private void HandleUserConnected(object sender, User e)
        {
            _server.Groups.GetOrCreateById(Guid.Empty).Users.Add(e);
        }
    }
}
