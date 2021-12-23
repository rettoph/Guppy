using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent.Lists.Interfaces;
using Guppy.Network;
using Guppy.Network.Security.EventArgs;
using Guppy.Network.Security.Lists;
using Guppy.Network.Services;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Example.Library.Scenes
{
    public class ExampleScene : Scene
    {
        #region Public Properties
        public Room Room { get; set; }
        #endregion

        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            this.Room = provider.GetService<RoomService>().GetById(0);
            this.Room.TryLinkScope(provider);

            this.Room.Users.OnUserAdded += this.HandleUserAdded;
            this.Room.Users.OnUserRemoved += this.HandleUserRemoved;

            this.Layers.Create<Layer>((l, _, _) => l.SetContext(Constants.LayerContexts.Foreground));
        }

        #region Frame Methods
        protected override void PreUpdate(GameTime gameTime)
        {
            base.PreUpdate(gameTime);

            this.Room.TryUpdate(gameTime);
        }
        #endregion

        #region Event Handlers
        private void HandleUserAdded(UserList sender, UserEventArgs args)
        {
            this.Room.Pipes[Guid.Empty].Users.TryAdd(args.User);
        }

        private void HandleUserRemoved(UserList sender, UserEventArgs args)
        {
            this.Room.Pipes[Guid.Empty].Users.TryRemove(args.User);
        }
        #endregion
    }
}
