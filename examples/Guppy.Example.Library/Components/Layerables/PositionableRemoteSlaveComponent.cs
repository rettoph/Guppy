using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent.Enums;
using Guppy.EntityComponent.Interfaces;
using Guppy.Example.Library.Layerables;
using Guppy.Example.Library.Messages;
using Guppy.Network.Attributes;
using Guppy.Network.Components;
using Guppy.Network.Enums;
using Guppy.Threading.Interfaces;
using Guppy.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Library.Components.Layerables
{
    [HostTypeRequired(HostType.Remote)]
    [NetworkAuthorizationRequired(NetworkAuthorization.Slave)]
    public sealed class PositionableRemoteSlaveComponent : NetworkComponent<Positionable>,
        IMessageProcessor<PositionDto>
    {
        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.Entity.OnPreUpdate += this.Update;

            this.Entity.Packets.RegisterProcessor<PositionDto>(this);
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            this.Entity.Packets.DeregisterProcessor<PositionDto>();

            this.Entity.OnPreUpdate -= this.Update;
        }
        #endregion

        #region Frame Methods
        private void Update(GameTime gameTime)
        {
            this.Entity.MasterPosition += this.Entity.Velocity * (Single)gameTime.ElapsedGameTime.TotalSeconds;

            if(Vector2.Distance(this.Entity.MasterPosition, this.Entity.Position) > 150)
            {
                this.Entity.Position = this.Entity.MasterPosition;
                return;
            }
            
            this.Entity.Position = Vector2.Lerp(this.Entity.Position, this.Entity.MasterPosition, Math.Min(1, (Single)(gameTime.ElapsedGameTime.TotalSeconds)));
        }
        #endregion

        #region Packet Processors
        void IMessageProcessor<PositionDto>.Process(PositionDto message)
        {
            this.Entity.MasterPosition = message.Position;
            this.Entity.Velocity = message.Velocity;
        }
        #endregion
    }
}
