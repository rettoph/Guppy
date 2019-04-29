using Guppy.Extensions;
using Guppy.Interfaces;
using Guppy.Loaders;
using Guppy.Network.Peers;
using Lidgren.Network;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pong.Client.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pong.Client.ServiceLoaders
{
    public class ClientPongGameServiceLoader : IServiceLoader
    {
        public void ConfigureServiceCollection(IServiceCollection services)
        {
            services.AddGame<ClientPongGame>();
            services.AddScene<ClientLoginScene>();
            services.AddScene<ClientLobbyScene>();
        }

        public void Boot(IServiceProvider provider)
        {
            var contentLoader = provider.GetLoader<ContentLoader>();
            contentLoader.Register("texture:ui:login:form", "Sprites/UI/login-form");
        }

        public void PreInitialize(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }

        public void Initialize(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }

        public void PostInitialize(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
