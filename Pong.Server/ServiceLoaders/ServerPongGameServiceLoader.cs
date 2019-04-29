using Guppy.Extensions;
using Guppy.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Pong.Server.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pong.Server.ServiceLoaders
{
    public class ServerPongGameServiceLoader : IServiceLoader
    {
        public void ConfigureServiceCollection(IServiceCollection services)
        {
            services.AddGame<ServerPongGame>();
            services.AddScene<ServerLobbyScene>();
        }

        public void Boot(IServiceProvider provider)
        {
            // throw new NotImplementedException();
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
