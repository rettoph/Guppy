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
using Guppy.Threading.Helpers;

namespace Guppy.Example.Client
{
    public class ClientExampleGame : ExampleGame
    {
        #region Private Fields
        private GraphicsDevice _graphics;
        private GraphicsDeviceManager _graphicsManager;
        private GameWindow _window;
        private ClientPeer _client;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _graphics);
            provider.Service(out _graphicsManager);
            provider.Service(out _window);
            provider.Service(out _client);
            
            _window.AllowUserResizing = true;
            
            _graphicsManager.PreferredBackBufferWidth = Guppy.Example.Library.Constants.WorldWidth;
            _graphicsManager.PreferredBackBufferHeight = Guppy.Example.Library.Constants.WorldHeight;
            _graphicsManager.ApplyChanges();
        }

        protected override void PostInitialize(ServiceProvider provider)
        {
            base.PostInitialize(provider);

            _client.TryStart();
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
