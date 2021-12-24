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
    internal sealed class PositionableRemoteSlaveComponent : NetworkComponent<Positionable>,
        IMessageProcessor<PositionDto>
    {
        #region Private Fields
        private Vector2 _masterPosition;
        #endregion

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
            Console.WriteLine($"{this.Entity.GetType().Name} - {_masterPosition}");
            _masterPosition += this.Entity.Velocity * (Single)gameTime.ElapsedGameTime.TotalSeconds;

            if(Vector2.Distance(_masterPosition, this.Entity.Position) > 20)
            {
                this.Entity.Position = _masterPosition;
                return;
            }
            
            this.Entity.Position = Vector2.Lerp(this.Entity.Position, _masterPosition, Math.Min(1, (Single)(gameTime.ElapsedGameTime.TotalSeconds)));
        }
        #endregion

        #region Packet Processors
        void IMessageProcessor<PositionDto>.Process(PositionDto message)
        {
            _masterPosition = message.Position;
            this.Entity.Velocity = message.Velocity;
        }
        #endregion
    }
}
