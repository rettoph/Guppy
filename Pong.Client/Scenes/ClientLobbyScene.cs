using Guppy.Network.Peers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public override void Draw(GameTime gameTime)
        {
            _graphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }
}
