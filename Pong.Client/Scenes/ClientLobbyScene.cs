using Guppy.Network.Groups;
using Guppy.Network.Peers;
using Guppy.UI.Entities;
using Guppy.UI.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pong.Library.Scenes;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Utilities.Units;
using Guppy.UI.Elements;
using Guppy.UI.StyleSheets;
using Guppy.UI.Enums;
using Guppy.Extensions;
using Guppy.Loaders;
using Guppy.Collections;
using Pong.Library.Layers;
using Guppy.Network.Security;
using Pong.Client.Elements;

namespace Pong.Client.Scenes
{
    public class ClientLobbyScene : LobbyScene
    {
        private GraphicsDevice _graphics;
        private SpriteFont _font;
        private SceneCollection _scenes;
        private ClientPeer _client;
        private ClientGroup _group;

        private LobbyChatWindow _chat;

        public ClientLobbyScene(SceneCollection scenes, GraphicsDevice graphics, ClientPeer client, IServiceProvider provider) : base(client, provider)
        {
            _graphics = graphics;
            _font = provider.GetLoader<ContentLoader>().Get<SpriteFont>("ui:font");
            _scenes = scenes;
            _client = client;
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.layers[0].Debug = true;

            _group = _client.Groups.GetById(Guid.Empty) as ClientGroup;

            var stage = this.entities.Create("ui:stage") as Stage;
            _chat = new LobbyChatWindow(stage);
        }

        public override void Draw(GameTime gameTime)
        {
            _graphics.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }
}
