using Guppy.Network.Peers;
using Guppy.UI.Entities;
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

        public ClientLobbyScene(GraphicsDevice graphicsDevice, ClientPeer client, IServiceProvider provider) : base(client, provider)
        {
            _graphicsDevice = graphicsDevice;
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.layers[0].Debug = false;

            var stage = this.entities.Create("ui:stage") as Stage;

            stage.Content.CreateElement<ChatWindow>(0, 0, 1f, 300);
        }

        public override void Draw(GameTime gameTime)
        {
            _graphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }
}
