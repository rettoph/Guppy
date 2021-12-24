using Guppy.EntityComponent.DependencyInjection;
using Guppy.Example.Library;
using Guppy.Example.Library.Scenes;
using Guppy.EntityComponent.Lists.Interfaces;
using Guppy.Network.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Example.Library.Layerables;
using Guppy.Network.Security.Lists;
using Guppy.Network.Security.EventArgs;
using Microsoft.Xna.Framework;

namespace Guppy.Example.Server.Scenes
{
    public class ServerExampleScene : ExampleScene
    {
        #region Private Fields
        private Ball _ball;
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            _ball = this.Layerables.Create<Ball>();

            this.Room.Users.OnUserAdded += this.HandleUserAdded;
        }
        #endregion

        #region Event Handlers
        private void HandleUserAdded(UserList sender, UserEventArgs args)
        {
            if(args.User.NetPeer is null)
            {
                return;
            }

            this.Layerables.Create<Paddle>((paddle, _, _) =>
            {
                paddle.User = args.User;
                paddle.Position = new Vector2(0, Library.Constants.WorldHeight - Paddle.Height);
            });
        }
        #endregion
    }
}
