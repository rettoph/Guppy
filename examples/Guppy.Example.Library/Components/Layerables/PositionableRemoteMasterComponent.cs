using Guppy.EntityComponent.DependencyInjection;
using Guppy.Example.Library.Layerables;
using Guppy.Example.Library.Messages;
using Guppy.Network.Attributes;
using Guppy.Network.Components;
using Guppy.Network.Enums;
using Guppy.Network.Messages;
using Guppy.Threading.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Library.Components.Layerables
{
    [HostTypeRequired(HostType.Remote)]
    [NetworkAuthorizationRequired(NetworkAuthorization.Master)]
    internal sealed class PositionableRemoteMasterComponent : NetworkComponent<Positionable>,
        IMessageFactory<PositionDto>
    {
        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            this.Entity.Packets.RegisterPacket<PositionDto, CreateNetworkEntityMessage>(this);
            this.Entity.Packets.RegisterPacket<PositionDto, PositionMessage>(this);
        }
        #endregion

        #region Packet Factories
        public PositionDto Create()
        {
            return new PositionDto(this.Entity.Position);
        }
        #endregion
    }
}
