using Guppy;
using Guppy.Network;
using Guppy.Network.Groups;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pong.Client.Scenes
{
    public class ClientGameScene : NetworkScene
    {
        private GraphicsDevice _graphicsDevice;
        private ClientGroup _group;

        public ClientGameScene(GraphicsDevice graphicsDevice, ClientGroup group, IServiceProvider provider) : base(provider)
        {
            _graphicsDevice = graphicsDevice;
            _group = group;
        }

        public override void Draw(GameTime gameTime)
        {
            _graphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }
}
