using Guppy;
using Guppy.Network.Groups;
using Guppy.Network.Peers;
using Guppy.Network.Security;
using Lidgren.Network;
using Pong.Library.Scenes;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Extensions.Lidgren;
using Pong.Server.Configurations;
using Microsoft.Extensions.Logging;

namespace Pong.Server.Scenes
{
    public class ServerLobbyScene : LobbyScene
    {
        private ServerPeer _server;
        private User _serverUser;
        private ServerGroup _serverGroup;
        private List<ServerPongGame> _games;
        private GuppyLoader _guppy;

        public ServerLobbyScene(GuppyLoader guppy, User serverUser, ServerPeer server, IServiceProvider provider) : base(server.Groups.GetOrCreateById(Guid.Empty), provider)
        {
            _guppy = guppy;
            _server = server;
            _serverUser = serverUser;
            // Add the server user first...
            this.group.Users.Add(_serverUser);

            // Bind to server events, auto adding new users to the lobby
            server.OnUserConnected += this.HandlerUserConnected;
            this.group.Users.Added += this.HandleUserJoined;
            this.group.Users.Removed += this.HandleUserLeft;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _serverGroup = this.group as ServerGroup;
            _games = new List<ServerPongGame>();

            this.group.MessageHandler.Add("create-game", this.HandleCreateGameMessage);
        }

        #region Message Handlers
        protected override void HandleChatMessage(NetIncomingMessage im)
        {
            var content = im.ReadString();
            var sender = this.group.Users.GetByNetConnection(im.SenderConnection);

            this.BroadcastChatMessage(sender, content);
        }

        private void BroadcastChatMessage(User sender, string content)
        {
            if (sender == default(User))
            {
                this.logger.LogWarning("Unable to broadcast chat message. Unknown or invalid sender.");
            }
            else
            {
                var om = this.group.CreateMessage("chat");
                om.Write(sender.Id);
                om.Write(content);

                if (_serverGroup.Connections.Count > 0)
                    this.group.SendMesssage(om);
            }
        }
        #endregion

        private void HandlerUserConnected(object sender, User e)
        {
            // Add the new user to the lobby group
            this.group.Users.Add(e);
        }

        private void HandleUserJoined(object sender, User e)
        {
            this.BroadcastChatMessage(_serverUser, $"{e.Get("name")} joined the lobby.");
        }

        private void HandleUserLeft(object sender, User e)
        {
            this.BroadcastChatMessage(_serverUser, $"{e.Get("name")} left the lobby.");
        }

        private void HandleCreateGameMessage(NetIncomingMessage im)
        {
            var user = this.group.Users.GetByNetConnection(im.SenderConnection);
            var game = _guppy.Games.Create<ServerPongGame>();
            var group = _server.Groups.GetOrCreateById(Guid.NewGuid()) as ServerGroup;
            game.SetScene(game.CreateScene<ServerGameScene>(new ServerGameSceneConfiguration()
            {
                Owner = user,
                Group = group
            }));
            game.RunAsync();
            

            var om = this.group.CreateMessage("join-game");
            om.Write(group.Id);
            // Send a message to the user, letting them know they have officially joined the game
            _serverGroup.SendMesssage(om, im.SenderConnection, NetDeliveryMethod.ReliableOrdered);
            // Remove the user from the lobby
            this.group.Users.Remove(user);
        }
    }
}
