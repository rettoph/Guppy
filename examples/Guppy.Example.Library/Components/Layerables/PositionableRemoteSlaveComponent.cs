using Guppy.EntityComponent.DependencyInjection;
using Guppy.Example.Library.Layerables;
using Guppy.Example.Library.Messages;
using Guppy.Network.Attributes;
using Guppy.Network.Components;
using Guppy.Network.Enums;
using Guppy.Threading.Interfaces;
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
        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            this.Entity.Packets.RegisterProcessor<PositionDto>(this);
        }
        #endregion

        #region Packet Processors
        void IMessageProcessor<PositionDto>.Process(PositionDto message)
        {
            this.Entity.Position = message.Position;
        }
        #endregion
    }
}
