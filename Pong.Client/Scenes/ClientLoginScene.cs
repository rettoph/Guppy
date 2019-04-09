using Guppy;
using Guppy.Collections;
using Guppy.Extensions;
using Guppy.Loaders;
using Guppy.Network.Peers;
using Guppy.Network.Security;
using Guppy.UI.Entities;
using Lidgren.Network;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pong.Library.Layers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Pong.Client.Scenes
{
    public class ClientLoginScene : Scene
    {
        private ClientPeer _client;
        private ContentLoader _content;
        private GraphicsDevice _graphicsDevice;
        private SceneCollection _scenes;

        public ClientLoginScene(SceneCollection scenes, GraphicsDevice graphicsDevice, ClientPeer client, IServiceProvider provider) : base(provider)
        {
            _scenes = scenes;
            _client = client;
            _content = provider.GetLoader<ContentLoader>();
            _graphicsDevice = graphicsDevice;
        }

        protected override void Initialize()
        {
            base.Initialize();

            var test = this.layers.Create<SimpleLayer>();
            test.Debug = true;

            var stage = this.entities.Create("ui:stage") as Stage;
        }

        public override void Draw(GameTime gameTime)
        {
            _graphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }
}
