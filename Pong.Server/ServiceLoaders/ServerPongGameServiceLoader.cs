using Guppy.Extensions;
using Guppy.Interfaces;
using Guppy.Network.Peers;
using Guppy.Network.Security;
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
            services.AddScene<ServerGameScene>();
            services.AddSingleton<User>(this.BuildServerUser);
        }

        public void Boot(IServiceProvider provider)
        {
            var peer = provider.GetRequiredService<Peer>();
            var sUser = provider.GetRequiredService<User>();

            // Create a new virtual user to represent the server from within games
            peer.Users.Add(sUser);
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

        private User BuildServerUser(IServiceProvider provider)
        {
            var peer = provider.GetRequiredService<Peer>();

            var sUser = new User(Guid.NewGuid(), peer.GetNetPeer().UniqueIdentifier);
            sUser.Set("name", "Server");
            sUser.Set("color", "255,0,0");

            return sUser;
        }
    }
}
