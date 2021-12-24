using Guppy.EntityComponent.DependencyInjection;
using Guppy.Example.Library.Messages;
using Guppy.Network;
using Guppy.Network.Messages;
using Guppy.Network.Services;
using Guppy.Threading.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Library.Layerables
{
    /// <summary>
    /// A simple entity capeable of having a position.
    /// </summary>
    public abstract class Positionable : NetworkLayerable
    {
        #region Protected Fields
        protected Vector2 position;
        protected Vector2 velocity;
        #endregion

        #region Public Properties
        public Vector2 Position
        {
            get => position;
            set => position = value;
        }
        public Vector2 Velocity
        {
            get => velocity;
            set => velocity = value;
        }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.LayerGroup = Constants.LayerContexts.Foreground.Group.GetValue();

            var room = provider.GetService<RoomService>().GetById(0);
            this.Pipe = room.Pipes.GetById(Guid.Empty);
        }
        #endregion
    }
}
