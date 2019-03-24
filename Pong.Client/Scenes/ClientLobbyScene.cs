using Guppy.Network.Groups;
using Guppy.Network.Peers;
using Guppy.UI.Entities;
using Guppy.UI.Layers;
using Guppy.UI.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pong.Client.Layers;
using Pong.Library.Scenes;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Utilities.Units;

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

            this.entities.Create("ui:image", new UnitRectangle(0, 0, 250, 1f), "texture:ui:lobby:sidebar");
            this.entities.Create("ui:image", new UnitRectangle(250, 0, new Unit[] { 1f, -250 }, 50), "texture:ui:lobby:header");
        }

        public override void Draw(GameTime gameTime)
        {
            _graphics.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }
}
