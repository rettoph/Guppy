using Guppy.Network.Groups;
using Guppy.Network.Peers;
using Guppy.UI.Entities;
using Guppy.UI.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pong.Client.Layers;
using Pong.Library.Scenes;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Utilities.Units;
using Guppy.UI.Elements;

namespace Pong.Client.Scenes
{
    public class ClientLobbyScene : LobbyScene
    {
        private GraphicsDevice _graphics;

        public ClientLobbyScene(GraphicsDevice graphics, Peer peer, IServiceProvider provider) : base(peer, provider)
        {
            _graphics = graphics;
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.layers.Create<SimpleLayer>();
            var stage = this.entities.Create("ui:stage") as Stage;
            var sidebar = stage.Content.Add(new Container(0, 0, 200, 1f)) as Container;
            var content = stage.Content.Add(new Container(200, 0, new Unit[] { 1f, -200 }, 1f)) as Container;

            content.Add(new Button(0.25f, 0.25f, 0.5f, 0.5f));
            content.Add(new Button(0.1f, 0.1f, 0.1f, 0.1f));
            content.Add(new Button(0.8f, 0.1f, 0.1f, 0.1f));
            content.Add(new Button(0.8f, 0.8f, 0.1f, 0.1f));
            content.Add(new Button(0.1f, 0.8f, 0.1f, 0.1f));

            sidebar.Add(new Button(25, 25, 150, 25));
            sidebar.Add(new Button(25, 75, 150, 25));
            sidebar.Add(new Button(25, 125, 150, 25));
            sidebar.Add(new Button(25, 175, 150, 25));
        }

        public override void Draw(GameTime gameTime)
        {
            _graphics.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }
}
