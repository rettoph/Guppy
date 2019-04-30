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

            // Start the server
            _server.Start();

            // Add a virtual user to represent the server
            var sUser = new User(Guid.NewGuid(), _server.GetNetPeer().UniqueIdentifier);
            sUser.Set("name", "Server");
            sUser.Set("color", "255,0,0");
            _server.Users.Add(sUser);

            this.Group.MessageHandler.Add("chat", this.HandleChatMessage);
            this.Group.Users.Add(_server.Users.GetByNetId(_server.GetNetPeer().UniqueIdentifier));

            _server.OnUserConnected += this.HandleUserConnected;
            this.Group.Users.Added += this.HandleUserJoined;
            this.Group.Users.Removed += this.HandleUserLeft;
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

        private void HandleUserLeft(object sender, User e)
        {
            var sUser = _server.Users.GetByNetId(_server.GetNetPeer().UniqueIdentifier);

            var om = this.Group.CreateMessage("chat");
            om.Write(sUser.Id);
            om.Write($"{e.Get("name")} left the lobby.");

            this.Group.SendMesssage(om, NetDeliveryMethod.ReliableOrdered);
        }

        private void HandleUserJoined(object sender, User e)
        {
            var sUser = _server.Users.GetByNetId(_server.GetNetPeer().UniqueIdentifier);

            var om = this.Group.CreateMessage("chat");
            om.Write(sUser.Id);
            om.Write($"{e.Get("name")} joined the lobby!");

            this.Group.SendMesssage(om, NetDeliveryMethod.ReliableOrdered);
        }
    }
}
