using Guppy.Example.Library;
using Guppy.EntityComponent.Lists.Interfaces;
using Guppy.Network.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.Security.Structs;
using Guppy.Network.Security.Enums;
using Guppy.Network.Security.Lists;
using Guppy.Network.Security.EventArgs;
using Guppy.Example.Library.Scenes;

namespace Guppy.Example.Server
{
    public class ServerExampleGame : ExampleGame
    {
        #region Private Fields
        private ServerPeer _server;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _server);

            _server.Users.OnUserAdded += this.HandleUserJoined;
            _server.Users.OnUserRemoved += this.HandleUserLeft;
        }

        protected override void PostInitialize(ServiceProvider provider)
        {
            base.PostInitialize(provider);


            _server.TryStart(1337, new[]
            {
                new Claim("name", "Server", ClaimType.Public)
            });
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            _server.Users.OnUserAdded -= this.HandleUserJoined;
            _server.Users.OnUserRemoved -= this.HandleUserLeft;

            _server = default;
        }
        #endregion

        #region Event Handlers
        private void HandleUserJoined(UserList sender, UserEventArgs args)
        {
            if(this.Scenes.Scene is ExampleScene scene)
            {
                scene.Room.Users.TryAdd(args.User);
            }
        }

        private void HandleUserLeft(UserList sender, UserEventArgs args)
        {
            if (this.Scenes.Scene is ExampleScene scene)
            {
                scene.Room.Users.TryRemove(args.User);
            }
        }
        #endregion
    }
}
