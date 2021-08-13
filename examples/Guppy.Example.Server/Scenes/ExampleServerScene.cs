using Guppy.DependencyInjection;
using Guppy.Example.Library;
using Guppy.Example.Library.Scenes;
using Guppy.Lists.Interfaces;
using Guppy.Network.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Example.Server.Scenes
{
    public class ExampleServerScene : ExampleScene
    {
        #region Lifecycle Methods
        protected override void Initialize(GuppyServiceProvider provider)
        {
            base.Initialize(provider);

            this.peer.Users.OnAdded += this.HandleUserConnected;
            this.Channel.Users.OnAdded += this.HandleUserJoined;

            this.Layerables.Create<Ball>();
        }
        #endregion

        #region Event Handlers
        private void HandleUserConnected(IServiceList<IUser> sender, IUser args)
        {
            this.Channel.Users.TryAdd(args);
            this.Channel.Pipes.GetOrCreateById(Guid.Empty).Users.TryAdd(args);
        }

        private void HandleUserJoined(IServiceList<IUser> sender, IUser args)
        {
            // throw new NotImplementedException();
        }
        #endregion
    }
}
