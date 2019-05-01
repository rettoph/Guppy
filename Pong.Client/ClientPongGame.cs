using Guppy.Extensions;
using Guppy.Loaders;
using Guppy.Network.Peers;
using Guppy.Network.Security;
using Lidgren.Network;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pong.Client.Scenes;
using Pong.Library;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pong.Client
{
    public class ClientPongGame : PongGame
    {
        private ClientPeer _client;

        public ClientPongGame(ILogger logger, IServiceProvider provider) : base(logger, provider)
        {
        }

        protected override void PreInitialize()
        {
            base.PreInitialize();
        }

        protected override void PostInitialize()
        {
            base.PostInitialize();

            // Save the games client peer
            _client = this.provider.GetService<ClientPeer>();

            // Create a new lobby scene
            this.SetScene(this.CreateScene<ClientLoginScene>());
        }

        public override void Update(GameTime gameTime)
        {
            _client.Update();

            base.Update(gameTime);
        }
    }
}
