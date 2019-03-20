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

            var test = this.entities.Create("ui:element", new Rectangle(100, 100, 100, 50)) as Element;
        }

        public override void Draw(GameTime gameTime)
        {
            _graphics.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}
