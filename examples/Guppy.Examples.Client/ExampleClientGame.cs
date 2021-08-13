using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Example.Library;
using Guppy.Network.Peers;
using System;
using System.Collections.Generic;
using Guppy.Network.Security;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Guppy.Examples.Client
{
    public class ExampleClientGame : ExampleGame
    {
        #region Private Fields
        private ClientPeer _client;
        private GraphicsDevice _graphics;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(GuppyServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _client);
            provider.Service(out _graphics);

            _client.Start();

            _client.TryConnect("127.0.0.1", 1337, new Claim("name", "Rettoph"));
        }
        #endregion

        protected override void PreDraw(GameTime gameTime)
        {
            base.PreDraw(gameTime);

            _graphics.Clear(Color.Black);
        }
    }
}
