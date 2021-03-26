using Guppy.DependencyInjection;
using Guppy.Example.Library;
using Guppy.Lists.Interfaces;
using Guppy.Network.Interfaces;
using Guppy.Network.Peers;
using Guppy.Network.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Example.Server
{
    public class ExampleServerGame : ExampleGame
    {
        #region Private Fields
        private ServerPeer _server;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _server);

            _server.CurrentUser.AddClaim(new Claim("name", "Server"));

            _server.Start();

            _server.Users.OnAdded += this.HandleUserJoined;
        }
        #endregion

        #region Event Handlers
        private void HandleUserJoined(IServiceList<IUser> sender, IUser user)
        {
            _server.Channels.GetOrCreate(0).Users.TryAdd(user);
        }
        #endregion
    }
}
