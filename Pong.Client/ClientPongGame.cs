using Guppy.Extensions;
using Guppy.Loaders;
using Guppy.Network.Peers;
using Guppy.Network.Security;
using Lidgren.Network;
using Microsoft.Extensions.DependencyInjection;
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
    class ClientPongGame : PongGame
    {
        private GraphicsDeviceManager _graphics;
        private GameWindow _window;
        private ContentManager _content;

        public ClientPongGame(GraphicsDeviceManager graphics, GameWindow window, ContentManager content)
        {
            _graphics = graphics;
            _window = window;
            _content = content;
        }

        protected override void Boot()
        {
            base.Boot();

            // Add the client services...
            this.services.AddSingleton<GraphicsDeviceManager>(_graphics);
            this.services.AddSingleton<GraphicsDevice>(_graphics.GraphicsDevice);
            this.services.AddSingleton<GameWindow>(_window);
            this.services.AddSingleton<ContentManager>(_content);
            this.services.AddSingleton<SpriteBatch>(new SpriteBatch(_graphics.GraphicsDevice));

            this.services.AddScene<TransitionScene>();
            this.services.AddScene<ClientLobbyScene>();
            this.services.AddScene<ClientLoginScene>();
        }

        protected override void PreInitialize()
        {
            base.PreInitialize();

            var contentLoader = this.provider.GetLoader<ContentLoader>();
            contentLoader.Register("texture:ui:login:form", "Sprites/UI/login-form");
            contentLoader.Register("texture:ui:login:input", "Sprites/UI/login-input");
            contentLoader.Register("texture:ui:login:input:active", "Sprites/UI/login-input-active");
            contentLoader.Register("texture:ui:login:button", "Sprites/UI/login-button");
            contentLoader.Register("texture:ui:login:button:hovered", "Sprites/UI/login-button-hovered");
            contentLoader.Register("texture:ui:login:button:pressed", "Sprites/UI/login-button-pressed");
        }

        protected override void PostInitialize()
        {
            base.PostInitialize();

            var client = this.provider.GetService<ClientPeer>();

            // Create a new lobby scene
            this.scenes.Create<ClientLoginScene>();
        }

        protected override Peer PeerFactory(IServiceProvider arg)
        {
            return new ClientPeer(this.config, this.logger);
        }
    }
}
