using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent.Enums;
using Guppy.EntityComponent.Interfaces;
using Guppy.Example.Library.Entities;
using Guppy.Example.Library.Messages;
using Guppy.Network;
using Guppy.Network.Attributes;
using Guppy.Network.Components;
using Guppy.Network.Enums;
using Guppy.Network.Messages;
using Guppy.Threading.Interfaces;
using Guppy.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Library.Components.Entities
{
    [HostTypeRequired(HostType.Remote)]
    [NetworkAuthorizationRequired(NetworkAuthorization.Master)]
    public sealed class PositionableRemoteMasterComponent : NetworkComponent<Positionable>,
        IMessageFactory<PositionDto>
    {
        #region Private Fields
        private IntervalInvoker _intervals;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _intervals);

            _intervals[Constants.Intervals.PositionMessage].OnInterval += this.HandlePositionMessageInterval;

            this.Entity.Packets.RegisterPacket<PositionDto, CreateNetworkEntityMessage>(this);
            this.Entity.Packets.RegisterPacket<PositionDto, PositionMessage>(this);
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            this.Entity.Packets.DeregisterPacket<PositionDto, CreateNetworkEntityMessage>(this);
            this.Entity.Packets.DeregisterPacket<PositionDto, PositionMessage>(this);

            _intervals[Constants.Intervals.PositionMessage].OnInterval -= this.HandlePositionMessageInterval;

            _intervals = default;
        }
        #endregion

        #region Packet Factories
        public PositionDto Create()
        {
            return new PositionDto(this.Entity);
        }
        #endregion

        #region Interval Handlers
        private void HandlePositionMessageInterval(GameTime gameTime)
        {
            this.Entity.SendMessage<PositionMessage>();
        }
        #endregion
    }
}
