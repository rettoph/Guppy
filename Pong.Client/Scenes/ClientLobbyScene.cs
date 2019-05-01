using Guppy.Extensions;
using Guppy.Loaders;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Network.Groups;
using Guppy.Network.Peers;
using Guppy.UI.Elements;
using Guppy.UI.Entities;
using Guppy.UI.Enums;
using Guppy.UI.Styles;
using Guppy.UI.Utilities.Units.UnitValues;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pong.Client.UI;
using Pong.Library.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pong.Client.Scenes
{
    public class ClientLobbyScene : LobbyScene
    {
        private GraphicsDevice _graphicsDevice;

        private Stage _stage;
        private ChatWindow _chatWindow;
        private ContentLoader _contentLoader;

        private TextButton _create;
        private Boolean _activeCreateRequest;

        private ClientPeer _client;

        public ClientLobbyScene(GraphicsDevice graphicsDevice, ClientPeer client, Group group, IServiceProvider provider) : base(group, provider)
        {
            _client = client;
            _graphicsDevice = graphicsDevice;
            _contentLoader = provider.GetLoader<ContentLoader>();
            _activeCreateRequest = false;
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.layers[0].Debug = false;

            _stage = this.entities.Create("ui:stage") as Stage;

            _stage.Content.CreateElement<UserList>(0, 0, 200, 1f);
            _chatWindow = _stage.Content.CreateElement<ChatWindow>(200, new UnitValue[] { 1f, -150 }, new UnitValue[] { 1f, -200 }, 150);

            _create = _stage.Content.CreateElement<TextButton>(200, new UnitValue[] { 1f, -180 }, new UnitValue[] { 1f, -200 }, 30, "Create Game");
            var games = _stage.Content.CreateElement<ScrollContainer>(200, 0, new UnitValue[] { 1f, -200 }, new UnitValue[] { 1f, -180 });
            games.Style.Set<Texture2D>(StateProperty.Background, _contentLoader.Get<Texture2D>("texture:ui:accent:2"));

            // Bind to UI element events
            _create.OnClicked += this.HandleCreateClicked;

            // Bind message types
            this.group.MessageHandler.Add("join-game", this.HandleJoinGameMessage);
        }

        #region Message Handlers
        protected override void HandleChatMessage(NetIncomingMessage im)
        {
            _chatWindow.Add(
                this.group.Users.GetById(im.ReadGuid()),
                im.ReadString());
        }

        private void HandleJoinGameMessage(NetIncomingMessage obj)
        {
            var gameGroup = _client.Groups.GetOrCreateById(obj.ReadGuid());
            this.game.SetScene(this.game.CreateScene<ClientGameScene>(group));
            this.Dispose();
        }

        private void HandleCreateClicked(object sender, TextButton e)
        {
            if (_activeCreateRequest)
            {
                _chatWindow.Add("Your previous request is being processed... Please wait.");
            }
            else {
                // Send create game request to server 
                var om = this.group.CreateMessage("create-game");
                this.group.SendMesssage(om);
                _activeCreateRequest = true;
            }

        }
        #endregion

        public override void Dispose()
        {
            base.Dispose();

            _stage.Dispose();
            this.group.Dispose();
        }
    }
}
