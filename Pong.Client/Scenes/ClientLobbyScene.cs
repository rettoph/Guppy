using Guppy.Network.Groups;
using Guppy.Network.Peers;
using Guppy.UI.Entities;
using Guppy.UI.Layers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pong.Client.Layers;
using Pong.Library.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

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

            this.layers.Create<UILayer>();

            var test = this.entities.Create("ui:button:lobby", "Create", new Rectangle(100, 100, 150, 45));
            this.entities.Create("ui:button:lobby:2", "Create", new Rectangle(100, 300, 150, 45));
            this.entities.Create("ui:button:lobby", "Create", new Rectangle(500, 300, 150, 45));
            this.entities.Create("ui:button:lobby", "Create", new Rectangle(200, 100, 550, 45));
        }

        public override void Draw(GameTime gameTime)
        {
            _graphics.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }
}
