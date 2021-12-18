using Guppy.Example.Library;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Guppy.Network.Security;
using Guppy.Network.Security.Structs;
using Guppy.Network.Security.Enums;
using Guppy.Network;
using Guppy.EntityComponent.DependencyInjection;

namespace Guppy.Examples.Client
{
    public class ExampleClientGame : ExampleGame
    {
        #region Private Fields
        private GraphicsDevice _graphics;
        private ClientPeer _client;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _graphics);
            provider.Service(out _client);
        }

        protected override void PostInitialize(ServiceProvider provider)
        {
            base.PostInitialize(provider);

            _client.TryStartAsync();
            _client.Connect("localhost", 1337, new Claim("name", "Rettoph", ClaimType.Public));
        }
        #endregion

        protected override void PreDraw(GameTime gameTime)
        {
            base.PreDraw(gameTime);

            _graphics.Clear(Color.Black);
        }
    }
}
